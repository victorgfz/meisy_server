namespace Meisy.Domain.Repositories.Order
{
    public interface IOrderWriteOnlyRepository
    {
        Task Add(Domain.Entities.Order order);
    }
}
