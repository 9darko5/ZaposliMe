using Microsoft.EntityFrameworkCore;
using ZaposliMe.Application.Common.Interfaces;
using ZaposliMe.Domain.Entities.Identity;
using ZaposliMe.Domain.Generic;
using ZaposliMe.Persistance;

namespace ZaposliMe.Domain.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(UserManagementDbContext context) : base(context) { }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
