namespace Meisy.Application.Services.Notifications;

public interface ICompanyNotificationService
{
    Task<bool> NotifyOrderCreated(int companyId, int orderId, DateTime deliveryDate);
    Task<bool> NotifyDeliveryReminder(int companyId, int orderId, DateTime deliveryDate);
}
