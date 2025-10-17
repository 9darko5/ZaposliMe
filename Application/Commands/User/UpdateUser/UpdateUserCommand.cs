using MediatR;

namespace ZaposliMe.Application.Commands.User.UpdateUser
{
    public record UpdateUserCommand(string Id, string FirstName, string LastName, string? PhoneNumber, long? Age) : IRequest
    {
    }
}
