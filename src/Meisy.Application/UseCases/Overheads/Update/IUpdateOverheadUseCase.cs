using Meisy.Communication.Requests.Overheads;

namespace Meisy.Application.UseCases.Overheads.Update
{
    public interface IUpdateOverheadUseCase
    {
        Task Execute(RequestUpdateOverheadJson request, int id);

    }
}
