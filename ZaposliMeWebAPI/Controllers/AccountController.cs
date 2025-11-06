using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ZaposliMe.Application.DTOs.Account;
using ZaposliMe.Domain.Entities.Identity;
using ZaposliMe.WebAPI.Models.Account;

namespace ZaposliMe.WebAPI.Controllers
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = "ZaposliMe";
        public string Audience { get; set; } = "ZaposliMe.SPA";
        public string Key { get; set; } = "";                 // strong 32+ chars
        public int AccessTokenMinutes { get; set; } = 60;      // 1h
        public int RefreshTokenDays { get; set; } = 7;         // 7 days
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private const string RefreshProvider = "ZaposliMe";
        private const string RefreshName = "RefreshToken";
        private const string RefreshExpName = "RefreshTokenExpiry";

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtOptions _jwt;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<JwtOptions> jwtOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwt = jwtOptions.Value;
        }

        // ---------- LOGIN (returns tokens) ----------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model, [FromQuery] bool useCookies = false)
        {
            // We ignore useCookies here and always issue tokens (frontend passes useCookies=false)
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return Unauthorized("Invalid login");

            var pwdOk = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
            if (!pwdOk.Succeeded)
                return Unauthorized("Invalid login");

            var roles = await _userManager.GetRolesAsync(user);
            var (access, expiresAt) = CreateAccessToken(user, roles);

            // Create and store refresh token (AspNetUserTokens)
            var refresh = CreateRefreshToken();
            await _userManager.SetAuthenticationTokenAsync(user, RefreshProvider, RefreshName, refresh);
            var refreshExpiry = DateTimeOffset.UtcNow.AddDays(_jwt.RefreshTokenDays).ToUnixTimeSeconds().ToString();
            await _userManager.SetAuthenticationTokenAsync(user, RefreshProvider, RefreshExpName, refreshExpiry);

            return Ok(new
            {
                access_token = access,
                token_type = "Bearer",
                expires_in = (int)(expiresAt - DateTimeOffset.UtcNow).TotalSeconds,
                refresh_token = refresh
            });
        }

        // ---------- LOGOUT (revoke refresh token) ----------
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] object _)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NoContent();

            await _userManager.RemoveAuthenticationTokenAsync(user, RefreshProvider, RefreshName);
            await _userManager.RemoveAuthenticationTokenAsync(user, RefreshProvider, RefreshExpName);
            return NoContent();
        }

        // ---------- REFRESH (issue new access token using refresh token) ----------
        public record RefreshRequest(string RefreshToken);

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.RefreshToken))
                return Unauthorized();

            // We need to find which user owns this refresh token
            // Approach: iterate by email from claims in expired access token if provided, or search by token (cheap approach uses Users list)
            // Better: store refresh token as authentication token and read via Users.
            // Here we scan; for large systems, keep an index/table. For your app size this is OK.

            // Try resolve by AccessToken "sub" (optional) — if client passes Authorization header
            User? user = null;

            // Fallback: scan users for token (works with EF in small apps)
            // If you have many users, replace with a proper store/index.
            foreach (var u in _userManager.Users)
            {
                var token = await _userManager.GetAuthenticationTokenAsync(u, RefreshProvider, RefreshName);
                if (token == req.RefreshToken)
                {
                    user = u;
                    break;
                }
            }

            if (user is null) return Unauthorized();

            var storedExp = await _userManager.GetAuthenticationTokenAsync(user, RefreshProvider, RefreshExpName);
            if (!long.TryParse(storedExp, out var expUnix)) return Unauthorized();
            if (DateTimeOffset.UtcNow > DateTimeOffset.FromUnixTimeSeconds(expUnix)) return Unauthorized();

            // Issue new access token (and rotate refresh token)
            var roles = await _userManager.GetRolesAsync(user);
            var (access, expiresAt) = CreateAccessToken(user, roles);

            var newRefresh = CreateRefreshToken();
            await _userManager.SetAuthenticationTokenAsync(user, RefreshProvider, RefreshName, newRefresh);
            var refreshExpiry = DateTimeOffset.UtcNow.AddDays(_jwt.RefreshTokenDays).ToUnixTimeSeconds().ToString();
            await _userManager.SetAuthenticationTokenAsync(user, RefreshProvider, RefreshExpName, refreshExpiry);

            return Ok(new
            {
                access_token = access,
                token_type = "Bearer",
                expires_in = (int)(expiresAt - DateTimeOffset.UtcNow).TotalSeconds,
                refresh_token = newRefresh
            });
        }

        // ---------- INFO (unchanged; used by your frontend) ----------
        [Authorize]
        [HttpGet("info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            var userInfo = new UserInfoDto
            {
                IsAuthenticated = true,
                Email = user.Email ?? string.Empty,
                Roles = roles.ToList(),
            };
            return Ok(userInfo);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age,
                Initials = $"{model.FirstName[0].ToString()}.{model.LastName[0].ToString()}."
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, model.Role);

            return Ok("User registered");
        }

        // ---------- helpers ----------
        private (string token, DateTimeOffset expiresAt) CreateAccessToken(User user, IEnumerable<string> roles)
        {
            var expiresAt = DateTimeOffset.UtcNow.AddMinutes(_jwt.AccessTokenMinutes);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(ClaimTypes.NameIdentifier,    user.Id),
                new(ClaimTypes.Name,              user.Email ?? user.UserName ?? user.Id),
                new(ClaimTypes.Email,             user.Email ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti,  Guid.NewGuid().ToString("N"))
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt.UtcDateTime,
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return (jwt, expiresAt);
        }

        private static string CreateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }
    }
}
