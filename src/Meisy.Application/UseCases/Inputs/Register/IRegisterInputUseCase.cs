using Meisy.Communication.Requests;
using Meisy.Communication.Responses;

namespace Meisy.Application.UseCases.Inputs.Register
{
    public interface IRegisterInputUseCase
    {
        Task<ResponseInputJson> Execute(RequestRegisterInputJson request);
    }
}
