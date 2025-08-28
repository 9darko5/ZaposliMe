using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZaposliMe.Application.Queries.User.GetUserByEmail;

namespace ZaposliMe.WebAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;
        public UserController(ISender sender) { 
            _sender = sender;
        }

        [HttpGet("/getUser")]
        public async Task<IActionResult> GetUser(string email)
        {
            var user = await _sender.Send(new GetUserByEmailQuery(email));

            if (user is null) 
                return NotFound();

            return Ok(user);
        }
    }
}
