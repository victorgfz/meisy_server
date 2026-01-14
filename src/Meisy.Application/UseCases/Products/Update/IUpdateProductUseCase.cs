using Meisy.Communication.Requests.Products;

namespace Meisy.Application.UseCases.Products.Update
{
    public interface IUpdateProductUseCase
    {
        Task Execute(RequestUpdateProductJson request, int id);
    }
}
