namespace Meisy.Application.UseCases.Products.Delete
{
    public interface IDeleteProductUseCase
    {
        Task Execute(int id);
    }
}
