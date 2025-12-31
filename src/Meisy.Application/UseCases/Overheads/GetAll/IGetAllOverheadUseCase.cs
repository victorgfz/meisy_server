using Meisy.Communication.Responses;

namespace Meisy.Application.UseCases.Overheads.GetAll
{
    public interface IGetAllOverheadUseCase
    {
        Task<List<ResponseOverheadJson>> Execute();
        
    }
}
