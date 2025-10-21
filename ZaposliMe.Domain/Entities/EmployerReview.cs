using System.ComponentModel.DataAnnotations.Schema;
using ZaposliMe.Domain.Primitives;

namespace ZaposliMe.Domain.Entities
{
    [Table("EmployerReview", Schema = "zaposlime")]
    public class EmployerReview : AggregateRoot
    {
        public string EmployerId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
