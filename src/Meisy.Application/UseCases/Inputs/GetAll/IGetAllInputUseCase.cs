using Meisy.Communication.Enums;
using Meisy.Communication.Responses.Inputs;

namespace Meisy.Application.UseCases.Inputs.GetAll
{
    public interface IGetAllInputUseCase
    {
        Task<List<ResponseInputJson>> Execute();
    }
}
