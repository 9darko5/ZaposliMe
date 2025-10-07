using MediatR;

namespace ZaposliMe.Application.Commands.Job.ApproveApplication
{
    public record WithdrawApplicationCommand(Guid ApplicationId, Guid JobId) : IRequest
    {
    }
}
