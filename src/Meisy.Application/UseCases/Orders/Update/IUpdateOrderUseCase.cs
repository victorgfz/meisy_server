using Meisy.Communication.Requests.Orders;

namespace Meisy.Application.UseCases.Orders.Update
{
    public interface IUpdateOrderUseCase
    {
        Task Execute(RequestUpdateOrderJson request, int id);
    }
}
