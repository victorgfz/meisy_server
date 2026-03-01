namespace Meisy.Domain.Repositories.Product
{
    public interface IProductWriteOnlyRepository
    {
        Task Add(Domain.Entities.Product product);
        void Delete(Domain.Entities.Product product);
        void DeleteProductInputs(List<Domain.Entities.ProductInput> productInputs);

    }
}
