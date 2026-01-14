using FluentValidation;
using Meisy.Communication.Requests.Products;
using Meisy.Exception;

namespace Meisy.Application.UseCases.Products.Register
{
    public class UpdateProductInputsValidator : AbstractValidator<RequestRegisterProductInputJson>
    {
        public UpdateProductInputsValidator()
        {
            RuleFor(pi => pi.InputId).GreaterThan(0).WithMessage(ResourceErrorMessages.INVALID_INPUT_ID);
            RuleFor(pi => pi.ProductionAmount).GreaterThan(0).WithMessage(ResourceErrorMessages.PRODUCTION_AMOUNT_LESS_THAN_ZERO);
            RuleFor(pi => pi.ProductionMeasurementUnit).IsInEnum().WithMessage(ResourceErrorMessages.INVALID_VALUE_FOR_PRODUCTION_MEASUREMENT_UNIT);
        }
    }
}
