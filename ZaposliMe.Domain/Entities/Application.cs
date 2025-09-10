using System.ComponentModel.DataAnnotations.Schema;
using ZaposliMe.Domain.Enums;

namespace ZaposliMe.Domain.Entities
{
    [Table("Application", Schema = "zaposlime")]
    public class Application
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public string EmployeeId { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime? AppliedAt { get; set; }
        public DateTime? StatusChangedAt { get; set; }
    }
}
