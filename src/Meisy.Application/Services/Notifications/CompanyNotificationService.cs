using System.Globalization;
using Meisy.Domain.Models;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.PushSubscription;
using Meisy.Domain.Services.WebPush;

namespace Meisy.Application.Services.Notifications;

public class CompanyNotificationService : ICompanyNotificationService
{
    private readonly IPushSubscriptionReadOnlyRepository _pushSubscriptionReadRepository;
    private readonly IPushSubscriptionWriteOnlyRepository _pushSubscriptionWriteRepository;
    private readonly IWebPushService _webPushService;
    private readonly IUnitOfWork _unitOfWork;

    public CompanyNotificationService(
        IPushSubscriptionReadOnlyRepository pushSubscriptionReadRepository,
        IPushSubscriptionWriteOnlyRepository pushSubscriptionWriteRepository,
        IWebPushService webPushService,
        IUnitOfWork unitOfWork)
    {
        _pushSubscriptionReadRepository = pushSubscriptionReadRepository;
        _pushSubscriptionWriteRepository = pushSubscriptionWriteRepository;
        _webPushService = webPushService;
        _unitOfWork = unitOfWork;
    }

    public async Task NotifyOrderCreated(int companyId, int orderId, DateTime deliveryDate)
    {
        var payload = new PushNotificationPayload
        {
            Title = "Novo pedido cadastrado",
            Message = $"Pedido #{orderId} com entrega em {FormatDeliveryDate(deliveryDate)}.",
            Type = "order.created",
            Url = "/dashboard/orders",
            OrderId = orderId,
        };

        await SendToCompany(companyId, payload);
    }

    public async Task NotifyDeliveryReminder(int companyId, int orderId, DateTime deliveryDate)
    {
        var payload = new PushNotificationPayload
        {
            Title = "Entrega se aproximando",
            Message = $"O pedido #{orderId} vence em {FormatDeliveryDate(deliveryDate)} e ainda nao foi iniciado.",
            Type = "order.delivery-reminder",
            Url = "/dashboard/orders",
            OrderId = orderId,
        };

        await SendToCompany(companyId, payload);
    }

    private async Task SendToCompany(int companyId, PushNotificationPayload payload)
    {
        try
        {
            var subscriptions = await _pushSubscriptionReadRepository.GetActiveByCompanyId(companyId);
            var hasRemovedSubscriptions = false;

            foreach (var subscription in subscriptions)
            {
                try
                {
                    var isValid = await _webPushService.SendNotificationAsync(subscription, payload);
                    if (!isValid)
                    {
                        _pushSubscriptionWriteRepository.Delete(subscription);
                        hasRemovedSubscriptions = true;
                    }
                }
                catch
                {
                    // Notification delivery must not block the business action that triggered it.
                }
            }

            if (hasRemovedSubscriptions)
            {
                await _unitOfWork.Commit();
            }
        }
        catch
        {
            // Notification storage/schema issues must not break order workflows.
        }
    }

    private static string FormatDeliveryDate(DateTime deliveryDate)
    {
        return deliveryDate.ToString("dd/MM/yyyy 'as' HH:mm", new CultureInfo("pt-BR"));
    }
}
