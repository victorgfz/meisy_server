using Meisy.Domain.Entities;
using Meisy.Domain.Models;

namespace Meisy.Domain.Services.WebPush
{
    public enum WebPushSendResult
    {
        Success,
        InvalidSubscription,
        Failed,
    }

    public interface IWebPushService
    {
        Task<WebPushSendResult> SendNotificationAsync(
            PushSubscription subscription,
            PushNotificationPayload payload);
    }
}
