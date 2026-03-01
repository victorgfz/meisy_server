using Meisy.Communication.Responses.Orders;

namespace Meisy.Application.UseCases.Orders.GetAll
{
    public interface IGetAllOrderUseCase
    {
        Task<List<ResponseDetailedOrderJson>> Execute();
    }
}
