using MediatR;

namespace ZaposliMe.Application.Commands.Job.CreateJob
{
    public record CreateJobApplicationCommand(string EmployeeId, Guid JobId) : IRequest<Guid>
    {
    }
}
