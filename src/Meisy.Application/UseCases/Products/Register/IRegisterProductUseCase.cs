using Meisy.Communication.Requests.Products;
using Meisy.Communication.Responses.Products;

namespace Meisy.Application.UseCases.Products.Register
{
    public interface IRegisterProductUseCase
    {
        Task<ResponseProductJson> Execute(RequestRegisterProductJson request);
    }
}
