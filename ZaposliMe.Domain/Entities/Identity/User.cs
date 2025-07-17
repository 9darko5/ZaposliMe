using Microsoft.AspNetCore.Identity;

namespace ZaposliMe.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public User()
        {
        }

        public string? Initials { get; set; }   
        
    }
}
