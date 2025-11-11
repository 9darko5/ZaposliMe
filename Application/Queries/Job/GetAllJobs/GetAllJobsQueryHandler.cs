using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.Enums;
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
            IQueryable<JobGridView> q = _context.JobGridViews
                .AsNoTracking()
                .Where(x => x.NumberOfWorkers >= 1);

            if (request.CityId is Guid cid)
                q = q.Where(x => x.CityId == cid);

            if (request.From is DateOnly from)
            {
                var fromDt = from.ToDateTime(TimeOnly.MinValue);
                q = q.Where(x => x.CreatedAt >= fromDt);
            }

            if (request.To is DateOnly to)
            {
                var toExclusive = to.ToDateTime(TimeOnly.MinValue).AddDays(1);
                q = q.Where(x => x.CreatedAt < toExclusive);
            }

            if (!string.IsNullOrWhiteSpace(request.UserId))
            {
                var userId = request.UserId!;
                q = q.Where(job => !_context.UserApplicationViews
                    .AsNoTracking()
                    .Any(a => a.JobId == job.Id && a.EmployeeId == userId && !a.Status.Equals(ApplicationStatus.Withdrawn)));
            }

            q = q.OrderByDescending(x => x.CreatedAt);

            return await q.ToListAsync(cancellationToken);
        }
    }
}
