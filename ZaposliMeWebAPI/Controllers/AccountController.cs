using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZaposliMe.Application.DTOs.Account;
using ZaposliMe.Domain.Entities.Identity;
using ZaposliMe.WebAPI.Models.Account;

namespace ZaposliMe.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("user")]
        public IActionResult GetUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Ok(new UserInfo { IsAuthenticated = false });
            }

            var userInfo = new UserInfo
            {
                IsAuthenticated = true,
                Email = User.Identity.Name,
                Roles = User.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList()
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
                return Unauthorized("Invalid login");

            return Ok("Login successful");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(object model)
        {
            await _signInManager.SignOutAsync();

            return Ok("Logout successful");
        }

        [Authorize]
        [HttpGet("info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);

            var userInfo = new UserInfoDto
            {
                IsAuthenticated = true,
                Email = user.Email,
                Roles = roles.ToList(),
            };

            return Ok(userInfo);
        }
    }
}
