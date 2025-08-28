using MediatR;
using ZaposliMe.Domain.ViewModels.Job;

namespace ZaposliMe.Application.Queries.Job.GetAllJobs
{
    public class GetAllJobsQuery : IRequest<List<JobGridView>>
    {
    }
}
