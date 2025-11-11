using FluentValidation;
using ZaposliMe.Application.DTOs.Account;

namespace ZaposliMe.WebAPI.Validators
{
    public sealed class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithErrorCode("EmailRequired")
                .EmailAddress().WithErrorCode("EmailInvalid");

            RuleFor(x => x.Password)
                .NotEmpty().WithErrorCode("PasswordRequired")
                .MinimumLength(6).WithErrorCode("PasswordTooShort");
        }
    }
}
