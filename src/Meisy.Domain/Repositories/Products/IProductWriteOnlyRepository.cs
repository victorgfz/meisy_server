namespace Meisy.Domain.Repositories.Products
{
    public interface IProductWriteOnlyRepository
    {
        Task Add(Domain.Entities.Product product);
        void Delete(Domain.Entities.Product product);
        void DeleteProductInputs(List<Domain.Entities.ProductInput> productInputs);

    }
}
