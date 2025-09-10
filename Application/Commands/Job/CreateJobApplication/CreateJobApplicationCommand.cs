using MediatR;

namespace ZaposliMe.Application.Commands.Job.CreateJob
{
    public record CreateJobApplicationCommand(string employeeId, Guid jobId) : IRequest<Guid>
    {
    }
}
