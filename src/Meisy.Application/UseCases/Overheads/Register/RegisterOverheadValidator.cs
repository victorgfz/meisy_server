using FluentValidation;
using Meisy.Communication.Requests.Overheads;
using Meisy.Exception;

namespace Meisy.Application.UseCases.Overheads.Register
{
    public class RegisterOverheadValidator : AbstractValidator<RequestRegisterOverheadJson>
    {
        public RegisterOverheadValidator()
        {
            RuleFor(overhead => overhead.Type).IsInEnum().WithMessage(ResourceErrorMessages.INVALID_VALUE_FOR_TYPE);
            RuleFor(overhead => overhead.CostPerHour).GreaterThanOrEqualTo(0).WithMessage(ResourceErrorMessages.COST_LESS_THAN_ZERO);
            RuleFor(overhead => overhead.CreatedAt).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_DATE);
            RuleFor(overhead => overhead.UpdatedAt).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_DATE);

        }
    }
}
