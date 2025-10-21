using ZaposliMe.Domain.Entities;
using ZaposliMe.Domain.Generic;
using ZaposliMe.Domain.Interfaces;
using ZaposliMe.Persistance;

namespace ZaposliMe.Domain.Repository
{
    public class EmployerReviewRepository : GenericRepository<EmployerReview>, IEmployerReviewRepository
    {
        public EmployerReviewRepository(ZaposliMeDbContext context) : base(context)
        {
        }
    }
}
