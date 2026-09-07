using FluentValidation;
using Meisy.Application.UseCases.Orders.Register;
using Meisy.Communication.Requests.Orders;
using Meisy.Exception;

namespace Meisy.Application.UseCases.Orders.Update
{
    public class UpdateOrderValidator : AbstractValidator<RequestUpdateOrderJson>
    {
        public UpdateOrderValidator()
        {
            RuleFor(order => order.OrderProducts).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_ORDER_PRODUCTS);
            RuleForEach(order => order.OrderProducts).SetValidator(new RegisterOrderProductsValidator());
            RuleFor(order => order.UpdatedAt).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_DATE);
        }
    }
}
