using MediatR;

namespace ZaposliMe.Application.Commands.Job.ApproveApplication
{
    public record ApproveApplicationCommand(Guid ApplicationId, Guid JobId) : IRequest
    {
    }
}
