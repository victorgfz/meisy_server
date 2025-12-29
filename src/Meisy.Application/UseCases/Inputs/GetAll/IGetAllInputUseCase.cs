using Meisy.Communication.Enums;
using Meisy.Communication.Responses;

namespace Meisy.Application.UseCases.Inputs.GetAll
{
    public interface IGetAllInputUseCase
    {
        Task<List<ResponseInputJson>> Execute(InputType type);
    }
}
