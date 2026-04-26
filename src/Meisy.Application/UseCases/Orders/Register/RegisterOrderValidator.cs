using FluentValidation;
using Meisy.Communication.Requests.Orders;
using Meisy.Domain.Entities;
using Meisy.Exception;

namespace Meisy.Application.UseCases.Orders.Register
{
    public class RegisterOrderValidator : AbstractValidator<RequestRegisterOrderJson>
    {
        public RegisterOrderValidator()
        {
            RuleFor(order => order.ClientId).GreaterThan(0).WithMessage(ResourceErrorMessages.INVALID_CLIENT_ID);
            RuleFor(order => order.DeliveryDate).GreaterThan(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_DATE);
            RuleFor(order => order.OrderProducts).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_ORDER_PRODUCTS);
            RuleForEach(order => order.OrderProducts).SetValidator(new RegisterOrderProductsValidator());

            RuleFor(order => order.CreatedAt).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_DATE);
            RuleFor(order => order.UpdatedAt).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_DATE);

        }
    }
}
