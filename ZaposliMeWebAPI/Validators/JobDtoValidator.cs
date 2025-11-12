using FluentValidation;
using ZaposliMe.Application.DTOs.Job;

namespace ZaposliMe.WebAPI.Validators
{
    public class JobDtoValidator : AbstractValidator<JobDto>
    {
        public JobDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("FieldRequired")
                .MaximumLength(100).WithMessage("JobTitleMaxLength");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("FieldRequired")
                .MaximumLength(1000).WithMessage("JobDescriptionMaxLength");

            RuleFor(x => x.NumberOfWorkers)
                .NotNull().WithMessage("FieldRequired")
                .InclusiveBetween(1, 1000).WithMessage("JobNumberOfWorkersRange");

            RuleFor(x => x.CityId)
                .NotNull().WithMessage("FieldRequired")
                .NotEqual(Guid.Empty).WithMessage("FieldRequired");
        }
    }
}
