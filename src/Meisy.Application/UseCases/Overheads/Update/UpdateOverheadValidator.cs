using FluentValidation;
using Meisy.Communication.Requests.Overheads;
using Meisy.Exception;

namespace Meisy.Application.UseCases.Overheads.Update
{
    public class UpdateOverheadValidator : AbstractValidator<RequestUpdateOverheadJson>
    {
        public UpdateOverheadValidator()
        {
            
            RuleFor(overhead => overhead.CostPerHour).GreaterThanOrEqualTo(0).WithMessage(ResourceErrorMessages.COST_LESS_THAN_ZERO);
            RuleFor(overhead => overhead.UpdatedAt).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_DATE);

        }
    }
}
