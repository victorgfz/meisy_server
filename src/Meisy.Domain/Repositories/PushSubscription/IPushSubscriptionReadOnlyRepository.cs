namespace Meisy.Domain.Repositories.PushSubscription
{
    public interface IPushSubscriptionReadOnlyRepository
    {
        Task<Meisy.Domain.Entities.PushSubscription?> GetByEndpoint(string endpoint);
        Task<List<Meisy.Domain.Entities.PushSubscription>> GetActiveByCompanyId(int companyId);
        Task<bool> Exists(int userId, string endpoint);
    }
}
