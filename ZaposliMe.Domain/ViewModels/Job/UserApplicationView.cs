using System.ComponentModel.DataAnnotations.Schema;
using ZaposliMe.Domain.Enums;

namespace ZaposliMe.Domain.ViewModels.Job
{

    [Table("UserApplicationView", Schema = "zaposlime")]
    public class UserApplicationView
    {
        public Guid Id { get; set; }
        public string EmployeeId { get; set; }
        public Guid JobId { get; set; }
        public ApplicationStatus Status { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string EmployerFullName { get; set; }
        public DateTime AppliedAt { get; set; }
        public DateTime JobCreatedAt { get; set; }
    }
}
