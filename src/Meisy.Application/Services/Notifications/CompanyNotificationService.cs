using System.Diagnostics;
using System.Globalization;
using Meisy.Domain.Models;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.PushSubscription;
using Meisy.Domain.Services.WebPush;
using Microsoft.Extensions.Logging;

namespace Meisy.Application.Services.Notifications;

public class CompanyNotificationService : ICompanyNotificationService
{
    private readonly IPushSubscriptionReadOnlyRepository _pushSubscriptionReadRepository;
    private readonly IPushSubscriptionWriteOnlyRepository _pushSubscriptionWriteRepository;
    private readonly IWebPushService _webPushService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompanyNotificationService> _logger;

    public CompanyNotificationService(
        IPushSubscriptionReadOnlyRepository pushSubscriptionReadRepository,
        IPushSubscriptionWriteOnlyRepository pushSubscriptionWriteRepository,
        IWebPushService webPushService,
        IUnitOfWork unitOfWork,
        ILogger<CompanyNotificationService> logger)
    {
        _pushSubscriptionReadRepository = pushSubscriptionReadRepository;
        _pushSubscriptionWriteRepository = pushSubscriptionWriteRepository;
        _webPushService = webPushService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> NotifyOrderCreated(int companyId, int orderId, DateTime deliveryDate)
    {
        var payload = new PushNotificationPayload
        {
            Title = "Novo pedido cadastrado",
            Message = $"Pedido #{orderId} com entrega em {FormatDeliveryDate(deliveryDate)}.",
            Type = "order.created",
            Url = "/dashboard/orders",
            OrderId = orderId,
        };

        return await SendToCompany(companyId, payload);
    }

    public async Task<bool> NotifyDeliveryReminder(int companyId, int orderId, DateTime deliveryDate)
    {
        var payload = new PushNotificationPayload
        {
            Title = "Entrega se aproximando",
            Message = $"O pedido #{orderId} vence em {FormatDeliveryDate(deliveryDate)} e ainda nao foi iniciado.",
            Type = "order.delivery-reminder",
            Url = "/dashboard/orders",
            OrderId = orderId,
        };

        return await SendToCompany(companyId, payload);
    }

    private async Task<bool> SendToCompany(int companyId, PushNotificationPayload payload)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var subscriptions = await _pushSubscriptionReadRepository.GetActiveByCompanyId(companyId);
            var hasRemovedSubscriptions = false;
            var successCount = 0;
            var invalidCount = 0;
            var failureCount = 0;

            foreach (var subscription in subscriptions)
            {
                try
                {
                    var result = await _webPushService.SendNotificationAsync(subscription, payload);
                    switch (result)
                    {
                        case WebPushSendResult.Success:
                            successCount++;
                            break;
                        case WebPushSendResult.InvalidSubscription:
                            _pushSubscriptionWriteRepository.Delete(subscription);
                            hasRemovedSubscriptions = true;
                            invalidCount++;
                            break;
                        case WebPushSendResult.Failed:
                            failureCount++;
                            break;
                    }
                }
                catch (System.Exception exception)
                {
                    failureCount++;
                    _logger.LogError(
                        exception,
                        "Unexpected error while sending push notification {NotificationType} to subscription {SubscriptionId}.",
                        payload.Type,
                        subscription.Id);
                }
            }

            if (hasRemovedSubscriptions)
            {
                await _unitOfWork.Commit();
            }

            _logger.LogInformation(
                "Push notification {NotificationType} completed for company {CompanyId} in {ElapsedMilliseconds} ms. Total: {TotalCount}, accepted: {SuccessCount}, invalid: {InvalidCount}, failed: {FailureCount}.",
                payload.Type,
                companyId,
                stopwatch.ElapsedMilliseconds,
                subscriptions.Count,
                successCount,
                invalidCount,
                failureCount);

            return failureCount == 0;
        }
        catch (System.Exception exception)
        {
            _logger.LogError(
                exception,
                "Push notification {NotificationType} could not be processed for company {CompanyId} because of a storage or configuration error.",
                payload.Type,
                companyId);
            return false;
        }
    }

    private static string FormatDeliveryDate(DateTime deliveryDate)
    {
        return deliveryDate.ToString("dd/MM/yyyy 'as' HH:mm", new CultureInfo("pt-BR"));
    }
}
