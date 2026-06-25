namespace Meisy.Domain.Repositories.PushSubscription
{
    public interface IPushSubscriptionWriteOnlyRepository
    {
        Task Add(Meisy.Domain.Entities.PushSubscription pushSubscription);
        void Update(Meisy.Domain.Entities.PushSubscription pushSubscription);
        void Delete(Meisy.Domain.Entities.PushSubscription pushSubscription);
    }
}
