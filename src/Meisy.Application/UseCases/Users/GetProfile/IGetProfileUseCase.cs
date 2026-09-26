using Meisy.Communication.Responses.Users;

namespace Meisy.Application.UseCases.Users.GetProfile
{
    public interface IGetProfileUseCase
    {
        Task<ResponseProfileJson> Execute();
    }
}
