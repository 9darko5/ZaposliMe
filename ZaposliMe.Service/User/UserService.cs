using ZaposliMe.Domain.Entities;
using ZaposliMe.Persistance;

namespace ZaposliMe.Service.User
{
    public partial class UserService : IUserService
    {
        private readonly ZaposliMeDbContext _zaposliMeDbContext;

        public UserService(ZaposliMeDbContext zaposliMeDbContext)
        {
            _zaposliMeDbContext = zaposliMeDbContext;
        }

        public void CreateUser(Guid aspNetUserId, string firstName, string lastName, long? age)
        {
            var userProfile = new UserProfile { AspNetUserId = aspNetUserId, FirstName = firstName, LastName = lastName, Age = age };

            _zaposliMeDbContext.UserProfiles.Add(userProfile);
            _zaposliMeDbContext.SaveChanges();
        }
    }
}
