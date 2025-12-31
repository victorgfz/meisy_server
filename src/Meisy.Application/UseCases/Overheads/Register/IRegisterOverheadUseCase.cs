using Meisy.Communication.Requests;
using Meisy.Communication.Responses;

namespace Meisy.Application.UseCases.Overheads.Register
{
    public interface IRegisterOverheadUseCase
    {
        Task<ResponseOverheadJson> Execute(RequestRegisterOverheadJson request);
    }
}
