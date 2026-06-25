namespace Meisy.Domain.Repositories.Order
{
    public interface IOrderReadOnlyRepository
    {
        Task<List<Domain.Entities.Order>> GetAll(int companyId);
        Task<Domain.Entities.Order?> GetByIdForUpdate(int companyId, int orderId);
        Task<List<Domain.Entities.Order>> GetAllByMonth(int companyId, DateTime date);
        Task<List<Domain.Entities.Order>> GetPendingOrdersForDeliveryReminder(DateTime now, DateTime reminderLimit);
        Task<List<Domain.Models.OrderProductIncidence>> GetTopProductsByMonth(int companyId, DateTime date, int top = 3);

    }
}
