using MediatR;

namespace ZaposliMe.Application.Commands.Job.CreateJob
{
    public record CreateJobCommand(string EmployerId, string? Title, string? Description, int? NumberOfWorkers, Guid CityId) : IRequest<Guid>
    {
    }
}
