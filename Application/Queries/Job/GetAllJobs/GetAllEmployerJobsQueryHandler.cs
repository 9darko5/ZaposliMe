using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.ViewModels.Job;
using ZaposliMe.Persistance;

namespace ZaposliMe.Application.Queries.Job.GetAllJobs
{
    public class GetAllEmployerJobsQueryHandler : IRequestHandler<GetAllEmployerJobsQuery, List<JobGridView>>
    {
        private readonly ZaposliMeQueryDbContext _context;

        public GetAllEmployerJobsQueryHandler(ZaposliMeQueryDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobGridView>> Handle(GetAllEmployerJobsQuery request, CancellationToken cancellationToken)
        {
            var jobs = await _context.JobGridViews.Where(x => x.EmployerId.Equals(request.employerId)).ToListAsync();
            return jobs;
        }
    }
}
