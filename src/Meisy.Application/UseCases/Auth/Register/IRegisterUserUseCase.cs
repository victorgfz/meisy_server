using Meisy.Communication.Requests;
using Meisy.Communication.Responses;

namespace Meisy.Application.UseCases.Auth.Register
{
    public interface IRegisterUserUseCase
    {
        Task<ResponseLoginJson> Execute(RequestRegisterUserJson request);
    }
}
