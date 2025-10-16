using System.ComponentModel.DataAnnotations.Schema;
using ZaposliMe.Domain.Primitives;

namespace ZaposliMe.Domain.Entities
{
    [Table("City", Schema = "zaposlime")]
    public class City : AggregateRoot
    {
        public string Name { get; set; }
        public string ZIP { get; set; }

        public ICollection<Job>? Jobs { get; set; } = new List<Job>();
    }
}
