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
    }
}
