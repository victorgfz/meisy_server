using FluentValidation;
using Meisy.Exception;
using Meisy.Communication.Requests.Auth;

namespace Meisy.Application.UseCases.Auth.Register
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
    {
        public RegisterUserValidator()
        {
            RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_NAME);
            RuleFor(user => user.Email)
               .NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_EMAIL)
               .EmailAddress()
               .When(user => string.IsNullOrWhiteSpace(user.Email) == false, ApplyConditionTo.CurrentValidator)
               .WithMessage(ResourceErrorMessages.INVALID_EMAIL);
            RuleFor(user => user.Password).Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$")
                .WithMessage(ResourceErrorMessages.INVALID_PASSWORD);
            RuleFor(user => user.CompanyCode).Matches("^[a-zA-Z0-9]{6}$")
                .WithMessage(ResourceErrorMessages.INVALID_COMPANY_ID);
        }
    }
}
