using ZaposliMe.Domain.Entities;
using ZaposliMe.Domain.Generic;
using ZaposliMe.Domain.Interfaces;
using ZaposliMe.Persistance;

namespace ZaposliMe.Domain.Repository
{
    public class EmployeeReviewRepository : GenericRepository<EmployeeReview>, IEmployeeReviewRepository
    {
        public EmployeeReviewRepository(ZaposliMeDbContext context) : base(context)
        {
        }
    }
}
