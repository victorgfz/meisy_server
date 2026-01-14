using Meisy.Communication.Requests.Overheads;
using Meisy.Communication.Responses.Overheads;

namespace Meisy.Application.UseCases.Overheads.Register
{
    public interface IRegisterOverheadUseCase
    {
        Task<ResponseOverheadJson> Execute(RequestRegisterOverheadJson request);
    }
}
