using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZaposliMe.Frontend.Resources;

namespace ZaposliMe.Frontend.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class PasswordRequiresUpperAttribute : ValidationAttribute
    {
        public PasswordRequiresUpperAttribute()
        {
            ErrorMessageResourceType = typeof(Resources.Resources);
            ErrorMessageResourceName = "PasswordRequiresUpper";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var s = value as string;

            // Ako je prazno, neka [Required] odradi svoje.
            if (string.IsNullOrEmpty(s) || s.Any(char.IsUpper))
                return ValidationResult.Success;

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
