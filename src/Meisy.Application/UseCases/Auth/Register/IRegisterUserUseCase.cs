using Meisy.Communication.Requests.Auth;
using Meisy.Communication.Responses.Auth;

namespace Meisy.Application.UseCases.Auth.Register
{
    public interface IRegisterUserUseCase
    {
        Task<ResponseLoginJson> Execute(RequestRegisterUserJson request);
    }
}
