namespace Meisy.Domain.Repositories.Product
{
    public interface IProductReadOnlyRepository
    {
        Task<List<Domain.Entities.Product>> GetAll(int companyId);
        Task<Domain.Entities.Product?> GetById(int companyId, int productId);
        Task<Domain.Entities.Product?> GetByIdForUpdate(int companyId, int productId);
        Task<bool> IsProductBeingUsed(int companyId, int productId);
    }
}
