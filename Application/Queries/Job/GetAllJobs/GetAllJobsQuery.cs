using MediatR;
using ZaposliMe.Domain.ViewModels.Job;

namespace ZaposliMe.Application.Queries.Job.GetAllJobs
{
    public record GetAllJobsQuery(Guid? CityId, DateOnly? From, DateOnly? To, string? UserId) : IRequest<List<JobGridView>>
    {
    }
}
