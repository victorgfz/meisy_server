using FluentValidation;
using Meisy.Communication.Requests;
using Meisy.Exception;

namespace Meisy.Application.UseCases.Inputs
{
    public class InputValidator : AbstractValidator<RequestInputJson>
    {
        public InputValidator()
        {
            RuleFor(input => input.Description).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_DESCRIPTION);
            RuleFor(input => input.Price).GreaterThan(0).WithMessage(ResourceErrorMessages.PRICE_LESS_THAN_ZERO);
            RuleFor(input => input.Type).IsInEnum().WithMessage(ResourceErrorMessages.INVALID_VALUE_FOR_TYPE);
            RuleFor(input => input.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_LESS_THAN_ZERO);
            RuleFor(input => input.MeasurementUnit).IsInEnum().WithMessage(ResourceErrorMessages.INVALID_VALUE_FOR_MEASUREMENT_UNIT);
            RuleFor(input => input.CreatedAt).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_DATE);
            RuleFor(input => input.UpdatedAt).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_DATE);
        }
    }
}
