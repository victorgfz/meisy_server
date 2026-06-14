using Meisy.Communication.Requests.Auth;
using Meisy.Communication.Responses.Auth;

namespace Meisy.Application.UseCases.Auth.RefreshToken
{
    public interface IRefreshTokenUseCase
    {
        Task<ResponseLoginJson> Execute(RequestRefreshTokenJson request);
    }
}
