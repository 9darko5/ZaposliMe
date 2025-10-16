using ZaposliMe.Domain.Entities;
using ZaposliMe.Domain.Generic;

namespace ZaposliMe.Domain.Interfaces
{
    public interface IJobRepository : IRepository<Job>
    {
        Task<Application> AddApplicationAsync(
            Guid jobId,
            string employeeId,
            CancellationToken cancellationToken = default);
    }
}
