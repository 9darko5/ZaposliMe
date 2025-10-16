using System.ComponentModel.DataAnnotations.Schema;

namespace ZaposliMe.Domain.ViewModels.Job
{
    [Table("JobGridView", Schema = "zaposlime")]
    public class JobGridView
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? NumberOfWorkers { get; set; }
        public string EmployerId { get; set; }
        public string EmployerFullName { get; set; }
        public Guid CityId { get; set; }
        public string CityName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
