using MediatR;

namespace ZaposliMe.Application.Commands.User.LeaveEmployerReview
{
    public record LeaveEmployerReviewCommand(int Rating, string? Comment, string EmployerId, string CreatedBy) : IRequest
    {
    }
}
