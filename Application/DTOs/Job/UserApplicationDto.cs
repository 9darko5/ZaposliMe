using ZaposliMe.Domain.Enums;

namespace ZaposliMe.Application.DTOs.Job
{
    public class UserApplicationDto
    {
        public ApplicationStatus Status { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string EmployerFullName { get; set; }
        public DateTime AppliedAt { get; set; }
        public DateTime JobCreatedAt { get; set; }
    }
}
