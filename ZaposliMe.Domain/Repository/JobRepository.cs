using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.Entities;
using ZaposliMe.Domain.Enums;
using ZaposliMe.Domain.Generic;
using ZaposliMe.Domain.Interfaces;
using ZaposliMe.Persistance;

namespace ZaposliMe.Domain.Repository
{
    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        public JobRepository(ZaposliMeDbContext context) : base(context)
        {
        }

        public async Task<Application> AddApplicationAsync(
            Guid jobId,
            string employeeId,
            CancellationToken ct = default)
        {
            // 1) Validate job exists (FK safety and better error)
            var jobExists = await _context.Set<Job>()
                .AsNoTracking()
                .AnyAsync(j => j.Id == jobId, ct);
            if (!jobExists)
                throw new InvalidOperationException($"Job {jobId} not found.");

            // 2) Duplicate guard (also add DB unique index on (JobId, EmployeeId))
            var alreadyApplied = await _context.Set<Application>()
                .AsNoTracking()
                .AnyAsync(a => a.JobId == jobId && a.EmployeeId == employeeId && !a.Status.Equals(ApplicationStatus.Withdrawn), ct);
            if (alreadyApplied)
                throw new InvalidOperationException("Employee already applied to this job.");

            // 3) Insert dependent directly (no Job tracked)
            var app = new Application
            {
                JobId = jobId,
                EmployeeId = employeeId,
                Status = ApplicationStatus.InReview,
                AppliedAt = DateTime.UtcNow
            };

            await _context.Set<Application>().AddAsync(app, ct);
            return app;
        }
    }
}
