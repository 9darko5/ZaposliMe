using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZaposliMe.Application.Commands.User.LeaveEmployerReview;
using ZaposliMe.Application.Commands.User.UpdateUser;
using ZaposliMe.Application.DTOs.User;
using ZaposliMe.Application.Queries.User.GetAllEmployerReviews;
using ZaposliMe.Application.Queries.User.GetAllEmployerReviewsByEmployerId;
using ZaposliMe.Application.Queries.User.GetUserByEmail;
using ZaposliMe.Domain.ViewModels.User;

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

        [HttpPost("leaveEmployerReview")]
        [Authorize]
        public async Task<IActionResult> LeaveEmployerReview(EmployerReviewDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();

            await _sender.Send(new LeaveEmployerReviewCommand(model.Rating, model.Comment, model.EmployerId, userId));

            return Ok();
        }

        [HttpGet("allEmployerReviews")]
        [Authorize]
        public async Task<IActionResult> GetAllEmployerReviews()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();

            var reviews = await _sender.Send(new GetAllEmployerReviewsQuery());

            return Ok(reviews);
        }

        [HttpGet("allEmployerReviewsById")]
        [Authorize]
        public async Task<IActionResult> GetAllEmployerReviewsById(string employerId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();

            var allGridReviews = await _sender.Send(new GetAllEmployerReviewsQuery());
            var allReviewsByEmployerId = await _sender.Send(new GetAllEmployerReviewsByEmployerIdQuery(employerId));

            var employerReview = allGridReviews.Where(r => r.EmployerId == employerId).FirstOrDefault() ?? new EmployerReviewGridView();

            employerReview.Reviews = allReviewsByEmployerId;

            return Ok(employerReview);
        }
    }
}
