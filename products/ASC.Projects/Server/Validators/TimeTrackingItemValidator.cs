using System;
using ASC.Projects.ViewModels;
using FluentValidation;

namespace ASC.Projects.Validators
{
    public class TimeTrackingItemValidator : AbstractValidator<TimeTrackingItemViewModel>
    {
        public TimeTrackingItemValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("Logging data must be provided");

            RuleFor(x => x.Date)
                .Must(v => v != DateTime.MinValue)
                .WithMessage("Logging Date must not be empty");

            RuleFor(x => x.PersonId)
                .Must(x => x != Guid.Empty)
                .WithMessage("Person Id must not be empty");
        }
    }
}
