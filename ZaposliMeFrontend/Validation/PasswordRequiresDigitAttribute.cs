using System.ComponentModel.DataAnnotations;

namespace ZaposliMe.Frontend.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class PasswordRequiresDigitAttribute : ValidationAttribute
    {
        public PasswordRequiresDigitAttribute()
        {
            ErrorMessageResourceType = typeof(Resources.Resources);
            ErrorMessageResourceName = "PasswordRequiresDigit";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var s = value as string;

            // ako je null/prazno, neka Required odradi svoje
            if (string.IsNullOrEmpty(s) || s.Any(char.IsDigit))
                return ValidationResult.Success;

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
