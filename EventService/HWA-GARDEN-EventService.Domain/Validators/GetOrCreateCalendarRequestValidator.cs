using FluentValidation;
using HWA.GARDEN.EventService.Domain.Requests;

namespace HWA.GARDEN.EventService.Domain.Validators
{
    public sealed class GetOrCreateCalendarRequestValidator : AbstractValidator<GetOrCreateCalendarRequest>
    {
        public GetOrCreateCalendarRequestValidator()
        {
            RuleFor(v => v.Name).NotEmpty();
            RuleFor(v => v.Year)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ValidationStrings.YearGreaterThanOrEqualToZero);
        }
    }
}
