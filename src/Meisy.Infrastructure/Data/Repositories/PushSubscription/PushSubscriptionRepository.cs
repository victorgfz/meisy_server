using Meisy.Domain.Repositories.PushSubscription;
using Microsoft.EntityFrameworkCore;

namespace Meisy.Infrastructure.Data.Repositories.PushSubscription;

public class PushSubscriptionRepository : IPushSubscriptionReadOnlyRepository, IPushSubscriptionWriteOnlyRepository
{
    private readonly MeisyDbContext _dbContext;

    public PushSubscriptionRepository(MeisyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Domain.Entities.PushSubscription pushSubscription)
    {
        await _dbContext.PushSubscriptions.AddAsync(pushSubscription);
    }

    public void Update(Domain.Entities.PushSubscription pushSubscription)
    {
        _dbContext.PushSubscriptions.Update(pushSubscription);
    }

    public void Delete(Domain.Entities.PushSubscription pushSubscription)
    {
        _dbContext.PushSubscriptions.Remove(pushSubscription);
    }

    public async Task<Domain.Entities.PushSubscription?> GetByEndpoint(string endpoint)
    {
        return await _dbContext.PushSubscriptions.FirstOrDefaultAsync(subscription => subscription.Endpoint == endpoint);
    }

    public async Task<List<Domain.Entities.PushSubscription>> GetActiveByCompanyId(int companyId)
    {
        return await _dbContext.PushSubscriptions
            .AsNoTracking()
            .Where(subscription =>
                subscription.ReceiveNotifications &&
                subscription.User.CompanyId == companyId)
            .ToListAsync();
    }

    public async Task<bool> Exists(int userId, string endpoint)
    {
        return await _dbContext.PushSubscriptions.AnyAsync(subscription =>
            subscription.UserId == userId &&
            subscription.Endpoint == endpoint);
    }
}
