namespace Meisy.Domain.Repositories.Order
{
    public interface IOrderReadOnlyRepository
    {
        Task<List<Domain.Entities.Order>> GetAll(int companyId);
        Task<Domain.Entities.Order?> GetByIdForUpdate(int companyId, int orderId);
    }
}
