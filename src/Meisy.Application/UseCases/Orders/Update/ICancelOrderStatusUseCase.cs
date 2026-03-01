namespace Meisy.Application.UseCases.Orders.Update
{
    public interface ICancelOrderStatusUseCase
    {
        Task Execute(int id);

    }
}
