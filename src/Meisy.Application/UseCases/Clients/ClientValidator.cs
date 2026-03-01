using FluentValidation;
using Meisy.Communication.Requests.Clients;
using Meisy.Exception;
using System.Text.RegularExpressions;

namespace Meisy.Application.UseCases.Clients
{
    public class ClientValidator : AbstractValidator<RequestClientJson>
    {
        public ClientValidator()
        {
            RuleFor(client => client.Name).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_NAME);
            RuleFor(client => client.Phone)
                .Cascade(CascadeMode.Stop)
                .Must(phone => Regex.Replace(phone!, @"\D", "").Length == 11)
                .When(client => !string.IsNullOrWhiteSpace(client.Phone))
                .WithMessage(ResourceErrorMessages.INVALID_PHONE);
            RuleFor(client => client.CreatedAt).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_DATE);
            RuleFor(client => client.UpdatedAt).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.INVALID_DATE);


        }
    }
}
