using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZaposliMe.Domain.ViewModels.User
{

    [Table("EmployerReviewGridView", Schema = "zaposlime")]
    public class EmployerReviewGridView
    {
        [Key]
        public string EmployerId { get; set; }
        public string EmployerFullName { get; set; }
        public long TotalReviews { get; set; }
        public int CommentCount { get; set; }
        public decimal AverageRating { get; set; }
        [NotMapped]
        public IList<EmployerReviewView> Reviews { get; set; }
    }
}
