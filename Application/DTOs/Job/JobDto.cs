namespace ZaposliMe.Application.DTOs.Job
{
    public class JobDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? NumberOfWorkers { get; set; }
    }
}
