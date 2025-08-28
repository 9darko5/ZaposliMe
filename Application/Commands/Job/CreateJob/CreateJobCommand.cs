using MediatR;

namespace ZaposliMe.Application.Commands.Job.CreateJob
{
    public record CreateJobCommand(string? Title, string? Description, int? NumberOfWorkers) : IRequest<Guid>
    {
    }
}
