using Meisy.Communication.Responses.Products;

namespace Meisy.Application.UseCases.Products.GetAll
{
    public interface IGetAllProductUseCase
    {
        Task<List<ResponseProductJson>> Execute();
    }
}
