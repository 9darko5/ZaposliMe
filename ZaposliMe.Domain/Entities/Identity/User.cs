using Microsoft.AspNetCore.Identity;

namespace ZaposliMe.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public User()
        {
        }

        public string? Initials { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? Age { get; set; }
        public ICollection<Application>? Applications { get; set; }
        public ICollection<Job>? Jobs { get; set; }
    }
}
