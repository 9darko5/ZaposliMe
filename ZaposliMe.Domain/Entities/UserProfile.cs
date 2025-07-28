using System.ComponentModel.DataAnnotations.Schema;
using ZaposliMe.Domain.Entities.Identity;
using ZaposliMe.Domain.Primitives;

namespace ZaposliMe.Domain.Entities
{
    [Table("UserProfile", Schema = "zaposlime")]

    public class UserProfile : EntityAudit
    {
        public UserProfile() 
        { }

        public Guid AspNetUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? Age { get; set; }
        public User User { get; set; }
    }
}
