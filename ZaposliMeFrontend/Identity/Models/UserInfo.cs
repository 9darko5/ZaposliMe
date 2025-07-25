namespace ZaposliMe.Frontend.Identity.Models
{
    public class UserInfo
    {
        public bool IsAuthenticated { get; set; }

        public string Email { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new();
    }
}
