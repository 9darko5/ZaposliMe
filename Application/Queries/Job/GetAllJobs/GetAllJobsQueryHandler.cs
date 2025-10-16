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
            // base query (only jobs with capacity)
            IQueryable<JobGridView> q = _context.JobGridViews
                .Where(x => x.NumberOfWorkers >= 1);

            // City filter
            if (request.CityId is Guid cid)
                q = q.Where(x => x.CityId == cid);

            // Date filters (CreatedAt assumed DateTime in the view)
            // from: inclusive; to: inclusive (implemented as < next day)
            if (request.From is DateOnly from)
            {
                var fromUtc = from.ToDateTime(TimeOnly.MinValue); // adjust if your CreatedAt is UTC/local
                q = q.Where(x => x.CreatedAt >= fromUtc);
            }

            if (request.To is DateOnly to)
            {
                var toExclusive = to.ToDateTime(TimeOnly.MinValue).AddDays(1);
                q = q.Where(x => x.CreatedAt < toExclusive);
            }

            // Order newest first
            q = q.OrderByDescending(x => x.CreatedAt);

            return await q.ToListAsync(cancellationToken);
        }
    }
}
