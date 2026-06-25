namespace Meisy.Application.Services.Notifications;

public interface ICompanyNotificationService
{
    Task NotifyOrderCreated(int companyId, int orderId, DateTime deliveryDate);
    Task NotifyDeliveryReminder(int companyId, int orderId, DateTime deliveryDate);
}
