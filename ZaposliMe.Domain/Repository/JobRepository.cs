using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.Entities;
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
    }
}
