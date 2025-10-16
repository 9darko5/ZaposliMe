using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.Entities;
using ZaposliMe.Domain.Generic;
using ZaposliMe.Domain.Interfaces;

namespace ZaposliMe.Domain.Repository
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        public CityRepository(DbContext context) : base(context)
        {
        }
    }
}
