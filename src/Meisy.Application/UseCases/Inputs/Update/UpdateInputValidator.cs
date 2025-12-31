using FluentValidation;
using Meisy.Communication.Requests;
using Meisy.Exception;

namespace Meisy.Application.UseCases.Inputs.Update
{
    public class UpdateInputValidator : AbstractValidator<RequestUpdateInputJson>
    {
        public UpdateInputValidator()
        {
            RuleFor(input => input.Description).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_DESCRIPTION);
            RuleFor(input => input.Price).GreaterThan(0).WithMessage(ResourceErrorMessages.PRICE_LESS_THAN_ZERO);
            RuleFor(input => input.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_LESS_THAN_ZERO);
            RuleFor(input => input.UpdatedAt).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_DATE);
        }
    }
}
