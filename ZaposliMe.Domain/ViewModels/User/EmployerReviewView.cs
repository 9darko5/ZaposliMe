using System.ComponentModel.DataAnnotations.Schema;

namespace ZaposliMe.Domain.ViewModels.User
{
    [Table("EmployerReviewView", Schema = "zaposlime")]
    public class EmployerReviewView
    {
        public Guid Id { get; set; }
        public string EmployerId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByFullName { get; set; }
    }
}
