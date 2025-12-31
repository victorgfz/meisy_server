using FluentValidation;
using Meisy.Communication.Requests;

namespace Meisy.Application.UseCases.Products.Register
{
    public class RegisterProductValidator : AbstractValidator<RequestRegisterProductJson>
    {
    }
}
