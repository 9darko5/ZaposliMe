namespace ZaposliMe.Application.DTOs.User
{
    public class EmployerReviewGridReviewDto
    {
        public string EmployerId { get; set; }
        public string EmployerFullName { get; set; }
        public long TotalReviews { get; set; }
        public long CommentCount { get; set; }
        public decimal AverageRating { get; set; }
    }
}
