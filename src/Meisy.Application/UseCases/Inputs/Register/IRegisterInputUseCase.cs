using Meisy.Communication.Requests.Inputs;
using Meisy.Communication.Responses.Inputs;

namespace Meisy.Application.UseCases.Inputs.Register
{
    public interface IRegisterInputUseCase
    {
        Task<ResponseInputJson> Execute(RequestRegisterInputJson request);
    }
}
