using Meisy.Communication.Responses.Clients;

namespace Meisy.Application.UseCases.Clients.GetAll
{
    public interface IGetAllClientUseCase
    {
        Task<List<ResponseClientJson>> Execute();
    }
}
