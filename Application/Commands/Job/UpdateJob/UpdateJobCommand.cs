using MediatR;

namespace ZaposliMe.Application.Commands.Job.UpdateJob
{
    public record UpdateJobCommand(Guid Id, string? Title, string? Description, int? NumberOfWorkers, Guid CityId) : IRequest
    {
    }
}
