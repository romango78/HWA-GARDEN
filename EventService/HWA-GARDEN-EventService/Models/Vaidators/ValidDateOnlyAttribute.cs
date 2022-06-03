using System.ComponentModel.DataAnnotations;

namespace HWA.GARDEN.EventService.Models.Vaidators
{
    internal sealed class ValidDateOnlyAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DateOnly date;
            if (!DateOnly.TryParse(value?.ToString(), out date))
            {
                return new ValidationResult("The provided value should be related to DateOnly format (e.g. 2022-01-01)."
                    , new[] { validationContext.MemberName });
            }
            return ValidationResult.Success;
        }
    }
}
