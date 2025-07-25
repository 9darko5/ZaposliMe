using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZaposliMe.WebAPI.Models.Account;

namespace ZaposliMe.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
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
    }
}
