using Meisy.Communication.Requests;

namespace Meisy.Application.UseCases.Inputs.Update
{
    public interface IUpdateInputUseCase
    {
        Task Execute(int id, RequestUpdateInputJson request);
    }
}
