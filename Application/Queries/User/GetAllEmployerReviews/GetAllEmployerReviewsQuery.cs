using MediatR;
using ZaposliMe.Domain.ViewModels.User;

namespace ZaposliMe.Application.Queries.User.GetAllEmployerReviews
{
    public record GetAllEmployerReviewsQuery : IRequest<List<EmployerReviewGridView>>
    {
    }
}
