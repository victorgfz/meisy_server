namespace Meisy.Domain.Repositories.Order
{
    public interface IOrderWriteOnlyRepository
    {
        Task Add(Domain.Entities.Order order);
        void DeleteOrderProducts(List<Domain.Entities.OrderProduct> orderProducts);
    }
}
