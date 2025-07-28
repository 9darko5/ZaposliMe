using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZaposliMe.Application.Shared.Account;
using ZaposliMe.Domain.Entities.Identity;
using ZaposliMe.Service.User;
using ZaposliMe.WebAPI.Models.Account;

namespace ZaposliMe.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
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
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Optionally add to role
            await _userManager.AddToRoleAsync(user, "User");

            _userService.CreateUser(Guid.Parse(user.Id), model.FirstName, model.LastName, model.Age);

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
    }
}
