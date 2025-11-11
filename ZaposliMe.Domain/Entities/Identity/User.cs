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
        public bool IsDeleted { get; set; }
        public ICollection<Application>? Applications { get; set; } = new List<Application>();
        public ICollection<EmployeeReview>? EmployeeReviews { get; set; } = new List<EmployeeReview>();
        public ICollection<EmployerReview>? EmployerReviews { get; set; } = new List<EmployerReview>();
        public ICollection<Job>? Jobs { get; set; }
    }
}
