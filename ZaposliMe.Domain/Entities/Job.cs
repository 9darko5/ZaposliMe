using System.ComponentModel.DataAnnotations.Schema;
using ZaposliMe.Domain.Primitives;

namespace ZaposliMe.Domain.Entities
{
    [Table("Job", Schema = "zaposlime")]
    public class Job : AggregateRoot
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? NumberOfWorkers { get; set; } 
    }
}
