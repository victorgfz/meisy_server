using FluentValidation;
using Meisy.Communication.Requests.Products;
using Meisy.Exception;

namespace Meisy.Application.UseCases.Products.Register
{
    public class UpdateProductValidator : AbstractValidator<RequestUpdateProductJson>
    {
        public UpdateProductValidator() {
            
            RuleFor(product => product.Description).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_DESCRIPTION);
            RuleFor(product => product.Price).GreaterThan(0).WithMessage(ResourceErrorMessages.PRICE_LESS_THAN_ZERO);
            RuleFor(product => product.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_LESS_THAN_ZERO);
            RuleFor(product => product.ProductionTime).GreaterThan(TimeSpan.Zero).WithMessage(ResourceErrorMessages.PRODUCTION_TIME_LESS_THAN_ZERO);
            RuleFor(product => product.Servings).GreaterThan(0).WithMessage(ResourceErrorMessages.SERVINGS_LESS_THAN_ZERO);
            RuleFor(product => product.ProductInputs).NotEmpty().WithMessage(ResourceErrorMessages. EMPTY_PRODUCT_INPUTS);
            RuleForEach(product => product.ProductInputs).SetValidator(new UpdateProductInputsValidator());


            
            RuleFor(product => product.UpdatedAt).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_DATE);




        }
    }
}
