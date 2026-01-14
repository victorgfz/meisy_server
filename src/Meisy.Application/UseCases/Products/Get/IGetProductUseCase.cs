using Meisy.Communication.Responses.Products;

namespace Meisy.Application.UseCases.Products.Get
{
    public interface IGetProductUseCase
    {
        Task<ResponseDetailedProductJson> Execute(int id);
    }
}
