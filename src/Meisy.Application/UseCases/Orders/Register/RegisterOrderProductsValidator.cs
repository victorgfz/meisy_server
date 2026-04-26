using FluentValidation;
using Meisy.Communication.Requests.Orders;
using Meisy.Exception;

namespace Meisy.Application.UseCases.Orders.Register
{
    public class RegisterOrderProductsValidator : AbstractValidator<RequestRegisterOrderProductJson>
    {
        public RegisterOrderProductsValidator()
        {
            RuleFor(op => op.ProductId).GreaterThan(0).WithMessage(ResourceErrorMessages.INVALID_PRODUCT_ID);
            RuleFor(op => op.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_LESS_THAN_ZERO);

        }
    }
}
