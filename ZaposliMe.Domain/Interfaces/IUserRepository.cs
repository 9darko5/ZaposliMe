using ZaposliMe.Domain.Entities.Identity;
using ZaposliMe.Domain.Generic;

namespace ZaposliMe.Application.Common.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
