using Meisy.Communication.Requests.Auth;
using Meisy.Communication.Responses.Auth;

namespace Meisy.Application.UseCases.Auth.Login
{
    public interface ILoginUseCase
    {
        Task<ResponseLoginJson> Execute(RequestLoginJson request);
    }
}
