using FluentValidation;
using HWA.GARDEN.EventService.Domain.Requests;

namespace HWA.GARDEN.EventService.Domain.Validators
{
    public sealed class GetEventListByPeriodQueryValidator : AbstractValidator<GetEventListByPeriodQuery>
    {
        public GetEventListByPeriodQueryValidator()
        {
            RuleFor(v => v.StartDate)
                .LessThanOrEqualTo(v => v.EndDate)
                .WithMessage(ValidationStrings.StartDateLessThanOrEqualToEndDate);
        }
    }
}
