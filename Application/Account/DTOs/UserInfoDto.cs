namespace ZaposliMe.Application.Account.DTOs
{
    public class UserInfoDto
    {
        public bool IsAuthenticated { get; set; }

        public string Email { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new();
    }
}
