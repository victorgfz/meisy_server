using System.Text.Json;
using Meisy.Domain.Models;
using Meisy.Domain.Services.WebPush;
using Microsoft.Extensions.Configuration;
using WebPush;
using DomainPushSubscription = Meisy.Domain.Entities.PushSubscription;

namespace Meisy.Infrastructure.Services.WebPush;

public class WebPushService : IWebPushService
{
    private readonly VapidDetails _vapidDetails;

    public WebPushService(IConfiguration configuration)
    {
        var subject = configuration["Settings:WebPush:Subject"] ?? "mailto:suporte@meisy.app";
        var publicKey = configuration["Settings:WebPush:PublicKey"] ?? string.Empty;
        var privateKey = configuration["Settings:WebPush:PrivateKey"] ?? string.Empty;

        _vapidDetails = new VapidDetails(subject, publicKey, privateKey);
    }

    public async Task<bool> SendNotificationAsync(
        DomainPushSubscription subscription,
        PushNotificationPayload payload)
    {
        if (string.IsNullOrWhiteSpace(subscription.Endpoint) ||
            string.IsNullOrWhiteSpace(subscription.P256DH) ||
            string.IsNullOrWhiteSpace(subscription.Auth))
        {
            return false;
        }

        var webPushSubscription = new global::WebPush.PushSubscription(
            subscription.Endpoint,
            subscription.P256DH,
            subscription.Auth);
        var payloadJson = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        var webPushClient = new WebPushClient();

        try
        {
            await webPushClient.SendNotificationAsync(webPushSubscription, payloadJson, _vapidDetails);
            return true;
        }
        catch (WebPushException exception)
        {
            if (exception.Message.Contains("404") || exception.Message.Contains("410"))
            {
                return false;
            }

            return true;
        }
    }
}
