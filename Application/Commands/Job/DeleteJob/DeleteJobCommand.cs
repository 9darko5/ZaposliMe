using MediatR;

namespace ZaposliMe.Application.Commands.Job.DeleteJob
{
    public record DeleteJobCommand(string Id) : IRequest
    {
    }
}
