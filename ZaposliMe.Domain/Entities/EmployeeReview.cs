using System.ComponentModel.DataAnnotations.Schema;
using ZaposliMe.Domain.Primitives;

namespace ZaposliMe.Domain.Entities
{
    [Table("EmployeeReview", Schema = "zaposlime")]
    public class EmployeeReview : AggregateRoot
    {
        public string EmployeeId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
