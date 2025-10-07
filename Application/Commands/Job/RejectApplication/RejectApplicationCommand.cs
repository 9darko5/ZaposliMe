using MediatR;

namespace ZaposliMe.Application.Commands.Job.ApproveApplication
{
    public record RejectApplicationCommand(Guid ApplicationId, Guid JobId) : IRequest
    {
    }
}
