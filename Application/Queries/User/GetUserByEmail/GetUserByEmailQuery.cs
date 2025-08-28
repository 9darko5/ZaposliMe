using MediatR;
using ZaposliMe.Domain.ViewModels;

namespace ZaposliMe.Application.Queries.User.GetUserByEmail
{
    public record GetUserByEmailQuery(string Email) : IRequest<UserGridView?>
    {
    }
}
