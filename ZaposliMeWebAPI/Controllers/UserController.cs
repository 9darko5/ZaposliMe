using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZaposliMe.Application.Commands.User.UpdateUser;
using ZaposliMe.Application.DTOs.User;
using ZaposliMe.Application.Queries.User.GetUserByEmail;

namespace ZaposliMe.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;
        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("getUser")]
        [Authorize]
        public async Task<IActionResult> GetUser(string email)
        {
            var user = await _sender.Send(new GetUserByEmailQuery(email));

            if (user is null)
                return NotFound();

            if (!user.Email.Equals(email))
                return Unauthorized();

            return Ok(user);
        }



        [HttpPut("updateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UserDetailsDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null || !userId.Equals(model.Id))
                return Unauthorized();

            await _sender.Send(new UpdateUserCommand(model.Id, model.FirstName, model.LastName, model.PhoneNumber, model.Age));

            return Ok();
        }
    }
}
