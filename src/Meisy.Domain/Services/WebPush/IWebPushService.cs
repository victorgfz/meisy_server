using Meisy.Domain.Entities;
using Meisy.Domain.Models;

namespace Meisy.Domain.Services.WebPush
{
    public interface IWebPushService
    {
        Task<bool> SendNotificationAsync(PushSubscription subscription, PushNotificationPayload payload);
    }
}
