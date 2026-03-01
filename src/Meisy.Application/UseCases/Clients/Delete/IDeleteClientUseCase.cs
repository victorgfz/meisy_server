namespace Meisy.Application.UseCases.Clients.Delete
{
    public interface IDeleteClientUseCase
    {
        Task Execute(int id);
    }
}
