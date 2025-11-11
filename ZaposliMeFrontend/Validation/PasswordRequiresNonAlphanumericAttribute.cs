using System.ComponentModel.DataAnnotations;
using System.Linq;
using ZaposliMe.Frontend.Resources;

namespace ZaposliMe.Frontend.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class PasswordRequiresNonAlphanumericAttribute : ValidationAttribute
    {
        public PasswordRequiresNonAlphanumericAttribute()
        {
            ErrorMessageResourceType = typeof(Resources.Resources);
            ErrorMessageResourceName = "PasswordRequiresNonAlphanumeric";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var s = value as string;

            // ako je null/prazno, neka Required odradi svoje
            if (string.IsNullOrEmpty(s) || s.Any(ch => !char.IsLetterOrDigit(ch)))
                return ValidationResult.Success;

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
