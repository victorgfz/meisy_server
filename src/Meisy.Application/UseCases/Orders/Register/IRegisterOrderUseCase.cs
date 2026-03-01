using Meisy.Communication.Requests.Orders;
using Meisy.Communication.Responses.Orders;

namespace Meisy.Application.UseCases.Orders.Register
{
    public interface IRegisterOrderUseCase
    {

        Task<ResponseOrderJson> Execute(RequestRegisterOrderJson request);
    }
}
