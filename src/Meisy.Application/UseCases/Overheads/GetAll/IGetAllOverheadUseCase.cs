using Meisy.Communication.Responses.Overheads;

namespace Meisy.Application.UseCases.Overheads.GetAll
{
    public interface IGetAllOverheadUseCase
    {
        Task<List<ResponseOverheadJson>> Execute();
        
    }
}
