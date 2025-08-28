using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.ViewModels.Job;
using ZaposliMe.Persistance;

namespace ZaposliMe.Application.Queries.Job.GetAllJobs
{
    public class GetAllJobsQueryHandler : IRequestHandler<GetAllJobsQuery, List<JobGridView>>
    {
        private readonly ZaposliMeQueryDbContext _context;

        public GetAllJobsQueryHandler(ZaposliMeQueryDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobGridView>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
        {
            var jobs = await _context.JobGridViews.ToListAsync();
            return jobs;
        }
    }
}
