using Meisy.Communication.Requests;
using Meisy.Communication.Responses;

namespace Meisy.Application.UseCases.Auth.Login
{
    public interface ILoginUseCase
    {
        Task<ResponseLoginJson> Execute(RequestLoginJson request);
    }
}
