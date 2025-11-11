using FluentValidation;
using ZaposliMe.Application.DTOs.Account;

namespace ZaposliMe.WebAPI.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator() 
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstNameRequired");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastNameRequired");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("EmailRequired")
                .EmailAddress().WithMessage("EmailInvalid");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("PasswordRequired")
                .MinimumLength(6).WithMessage("PasswordTooShort")      
                .Matches(@"[^a-zA-Z0-9]").WithMessage("PasswordRequiresNonAlphanumeric")
                .Matches(@"\d").WithMessage("PasswordRequiresDigit")
                .Matches(@"[A-Z]").WithErrorCode("PasswordRequiresUpper");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("PhoneRequired");
        }
    }
}
