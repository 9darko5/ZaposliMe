namespace ZaposliMe.Application.Shared.Account
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? Age { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
