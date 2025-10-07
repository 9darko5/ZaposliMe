using System.ComponentModel.DataAnnotations.Schema;
using ZaposliMe.Domain.Enums;

namespace ZaposliMe.Domain.ViewModels.Job
{

    [Table("EmployerApplicationView", Schema = "zaposlime")]
    public class EmployerApplicationView
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string EmployerId { get; set; }
        public ApplicationStatus Status { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string EmployeeFullName { get; set; }
        public DateTime? AppliedAt { get; set; }
        public DateTime? StatusChangedAt { get; set; }
        public DateTime JobCreatedAt { get; set; }
    }
}
