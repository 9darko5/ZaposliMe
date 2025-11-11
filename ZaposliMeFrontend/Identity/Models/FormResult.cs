using ZaposliMe.Application.DTOs;

namespace ZaposliMe.Frontend.Identity.Models
{
    public sealed class FormResult
    {
        public bool Succeeded { get; set; }
        public List<ValidationErrorDto> ErrorList { get; set; } = new();
    }
}
