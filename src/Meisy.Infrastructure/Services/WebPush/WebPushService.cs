using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Meisy.Domain.Models;
using Meisy.Domain.Services.WebPush;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebPush;
using DomainPushSubscription = Meisy.Domain.Entities.PushSubscription;

namespace Meisy.Infrastructure.Services.WebPush;

public sealed class WebPushService : IWebPushService, IDisposable
{
    private const int MaxAttempts = 3;
    private static readonly TimeSpan AttemptTimeout = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan[] RetryDelays =
    [
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(2),
    ];

    private readonly VapidDetails _vapidDetails;
    private readonly WebPushClient _webPushClient = new();
    private readonly ILogger<WebPushService> _logger;

    public WebPushService(
        IConfiguration configuration,
        ILogger<WebPushService> logger)
    {
        _logger = logger;

        var subject = configuration["Settings:WebPush:Subject"] ?? "mailto:suporte@meisy.app";
        var publicKey = configuration["Settings:WebPush:PublicKey"] ?? string.Empty;
        var privateKey = configuration["Settings:WebPush:PrivateKey"] ?? string.Empty;

        _vapidDetails = new VapidDetails(subject, publicKey, privateKey);
    }

    public async Task<WebPushSendResult> SendNotificationAsync(
        DomainPushSubscription subscription,
        PushNotificationPayload payload)
    {
        if (string.IsNullOrWhiteSpace(subscription.Endpoint) ||
            string.IsNullOrWhiteSpace(subscription.P256DH) ||
            string.IsNullOrWhiteSpace(subscription.Auth))
        {
            _logger.LogWarning(
                "Push subscription {SubscriptionId} has incomplete encryption data and will be removed.",
                subscription.Id);
            return WebPushSendResult.InvalidSubscription;
        }

        var webPushSubscription = new global::WebPush.PushSubscription(
            subscription.Endpoint,
            subscription.P256DH,
            subscription.Auth);
        var payloadJson = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        var options = new Dictionary<string, object>
        {
            ["vapidDetails"] = _vapidDetails,
            ["headers"] = new Dictionary<string, object>
            {
                ["Urgency"] = "high",
            },
        };

        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                using var timeoutCancellation = new CancellationTokenSource(AttemptTimeout);
                await _webPushClient.SendNotificationAsync(
                    webPushSubscription,
                    payloadJson,
                    options,
                    timeoutCancellation.Token);

                _logger.LogInformation(
                    "Push notification {NotificationType} accepted for subscription {SubscriptionId} on attempt {Attempt} in {ElapsedMilliseconds} ms.",
                    payload.Type,
                    subscription.Id,
                    attempt,
                    stopwatch.ElapsedMilliseconds);

                return WebPushSendResult.Success;
            }
            catch (WebPushException exception) when (IsInvalidSubscription(exception.StatusCode))
            {
                _logger.LogWarning(
                    "Push subscription {SubscriptionId} is no longer valid. Status code: {StatusCode}.",
                    subscription.Id,
                    (int)exception.StatusCode);
                return WebPushSendResult.InvalidSubscription;
            }
            catch (WebPushException exception) when (IsTransient(exception.StatusCode))
            {
                if (attempt < MaxAttempts)
                {
                    LogRetry(subscription.Id, payload.Type, attempt, (int)exception.StatusCode, stopwatch.ElapsedMilliseconds);
                    await Task.Delay(RetryDelays[attempt - 1]);
                    continue;
                }

                _logger.LogError(
                    exception,
                    "Push notification {NotificationType} failed for subscription {SubscriptionId} after {AttemptCount} attempts. Last status code: {StatusCode}.",
                    payload.Type,
                    subscription.Id,
                    MaxAttempts,
                    (int)exception.StatusCode);
                return WebPushSendResult.Failed;
            }
            catch (WebPushException exception)
            {
                _logger.LogError(
                    exception,
                    "Push notification {NotificationType} was rejected for subscription {SubscriptionId}. Status code: {StatusCode}.",
                    payload.Type,
                    subscription.Id,
                    (int)exception.StatusCode);
                return WebPushSendResult.Failed;
            }
            catch (Exception exception) when (exception is HttpRequestException or OperationCanceledException)
            {
                if (attempt < MaxAttempts)
                {
                    LogRetry(subscription.Id, payload.Type, attempt, null, stopwatch.ElapsedMilliseconds);
                    await Task.Delay(RetryDelays[attempt - 1]);
                    continue;
                }

                _logger.LogError(
                    exception,
                    "Push notification {NotificationType} failed for subscription {SubscriptionId} after {AttemptCount} attempts because of a network error or timeout.",
                    payload.Type,
                    subscription.Id,
                    MaxAttempts);
                return WebPushSendResult.Failed;
            }
        }

        return WebPushSendResult.Failed;
    }

    public void Dispose()
    {
        _webPushClient.Dispose();
    }

    private void LogRetry(
        int subscriptionId,
        string notificationType,
        int attempt,
        int? statusCode,
        long elapsedMilliseconds)
    {
        _logger.LogWarning(
            "Push notification {NotificationType} failed temporarily for subscription {SubscriptionId} on attempt {Attempt} in {ElapsedMilliseconds} ms. Status code: {StatusCode}. Retrying.",
            notificationType,
            subscriptionId,
            attempt,
            elapsedMilliseconds,
            statusCode);
    }

    private static bool IsInvalidSubscription(HttpStatusCode statusCode)
    {
        return statusCode is HttpStatusCode.NotFound or HttpStatusCode.Gone;
    }

    private static bool IsTransient(HttpStatusCode statusCode)
    {
        var numericStatusCode = (int)statusCode;
        return statusCode is HttpStatusCode.RequestTimeout or HttpStatusCode.TooManyRequests ||
               numericStatusCode >= 500;
    }
}
