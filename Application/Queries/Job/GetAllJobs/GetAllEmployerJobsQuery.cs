using MediatR;
using ZaposliMe.Domain.ViewModels.Job;

namespace ZaposliMe.Application.Queries.Job.GetAllJobs
{
    public record GetAllEmployerJobsQuery(string EmployerId) : IRequest<List<JobGridView>>
    {
    }
}
