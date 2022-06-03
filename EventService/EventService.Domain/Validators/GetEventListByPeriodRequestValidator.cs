using FluentValidation;
using HWA.GARDEN.EventService.Domain.Requests;

namespace HWA.GARDEN.EventService.Domain.Validators
{
    public class GetEventListByPeriodRequestValidator : AbstractValidator<GetEventListByPeriodQuery>
    {
        public GetEventListByPeriodRequestValidator()
        {
            RuleFor(v => v.StartDate)
                .LessThanOrEqualTo(v => v.EndDate)
                .WithMessage("The Start date should be equal or less than End date.");
        }
    }
}
