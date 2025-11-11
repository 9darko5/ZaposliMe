using MediatR;

namespace ZaposliMe.Application.Commands.User.DeleteUser
{
    public record DeleteUserCommand(string Id) : IRequest
    {
    }
}
