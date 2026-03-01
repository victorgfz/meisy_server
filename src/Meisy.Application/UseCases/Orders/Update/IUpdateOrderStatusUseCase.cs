using Meisy.Communication.Requests.Orders;

namespace Meisy.Application.UseCases.Orders.Update;

public interface IUpdateOrderStatusUseCase
{
    Task Execute(int id);
}