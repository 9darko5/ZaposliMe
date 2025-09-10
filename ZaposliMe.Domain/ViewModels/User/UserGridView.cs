using System.ComponentModel.DataAnnotations.Schema;

namespace ZaposliMe.Domain.ViewModels.User
{
    [Table("UserGridView", Schema = "identity")]
    public class UserGridView
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public long? Age { get; set; }
    }
}
