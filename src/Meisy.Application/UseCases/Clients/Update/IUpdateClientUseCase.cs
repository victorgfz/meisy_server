using Meisy.Communication.Requests.Clients;

namespace Meisy.Application.UseCases.Clients.Update
{
    public interface IUpdateClientUseCase
    {
        Task Execute(RequestClientJson request, int id);
    }
}
