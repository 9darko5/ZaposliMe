using MediatR;

namespace ZaposliMe.Application.Commands.Job.CreateJob
{
    public record CreateJobCommand(string employerId, string? Title, string? Description, int? NumberOfWorkers) : IRequest<Guid>
    {
    }
}
