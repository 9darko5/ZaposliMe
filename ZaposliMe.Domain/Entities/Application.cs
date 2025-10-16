using System.ComponentModel.DataAnnotations.Schema;
using ZaposliMe.Domain.Enums;
using ZaposliMe.Domain.Primitives;

namespace ZaposliMe.Domain.Entities
{
    [Table("Application", Schema = "zaposlime")]
    public class Application : Entity
    {
        public Guid JobId { get; set; }
        public string EmployeeId { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime? AppliedAt { get; set; }
        public DateTime? StatusChangedAt { get; set; }
        public virtual Job Job { get; internal set; }
    }
}
