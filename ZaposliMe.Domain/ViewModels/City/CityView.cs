using System.ComponentModel.DataAnnotations.Schema;

namespace ZaposliMe.Domain.ViewModels.City
{
    [Table("CityView", Schema = "zaposlime")]
    public class CityView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ZIP { get; set; }
    }
}
