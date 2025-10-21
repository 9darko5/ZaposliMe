using MediatR;
using ZaposliMe.Domain.ViewModels.User;

namespace ZaposliMe.Application.Queries.User.GetAllEmployerReviewsByEmployerId
{
    public record GetAllEmployerReviewsByEmployerIdQuery(string EmployerId) : IRequest<List<EmployerReviewView>>
    {
    }
}
