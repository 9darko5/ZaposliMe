using MediatR;

namespace ZaposliMe.Application.Commands.Job.DeleteJob
{
    public record DeleteJobCommand(Guid Id) : IRequest
    {
    }
}
