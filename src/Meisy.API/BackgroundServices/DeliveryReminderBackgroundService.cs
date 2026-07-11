using Meisy.Application.Services.Notifications;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Order;

namespace Meisy.API.BackgroundServices;

public class DeliveryReminderBackgroundService : BackgroundService
{
    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(15);
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<DeliveryReminderBackgroundService> _logger;

    public DeliveryReminderBackgroundService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<DeliveryReminderBackgroundService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SendDeliveryReminders(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (System.Exception exception)
            {
                _logger.LogError(exception, "Error while sending delivery reminder notifications.");
            }

            await Task.Delay(CheckInterval, stoppingToken);
        }
    }

    private async Task SendDeliveryReminders(CancellationToken cancellationToken)
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        var orderReadRepository = scope.ServiceProvider.GetRequiredService<IOrderReadOnlyRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var companyNotificationService = scope.ServiceProvider.GetRequiredService<ICompanyNotificationService>();

        var now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
            DateTime.UtcNow,
            "E. South America Standard Time");
        var reminderLimit = now.AddHours(24);
        var orders = await orderReadRepository.GetPendingOrdersForDeliveryReminder(now, reminderLimit);

        if (orders.Count == 0)
        {
            return;
        }

        _logger.LogInformation(
            "Found {OrderCount} pending delivery reminders between {Now} and {ReminderLimit}.",
            orders.Count,
            now,
            reminderLimit);

        var hasProcessedOrders = false;

        foreach (var order in orders)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var wasProcessed = await companyNotificationService.NotifyDeliveryReminder(
                order.CompanyId,
                order.Id,
                order.DeliveryDate);

            if (!wasProcessed)
            {
                _logger.LogWarning(
                    "Delivery reminder for order {OrderId} was not marked as sent because at least one push delivery failed. It will be retried on the next cycle.",
                    order.Id);
                continue;
            }

            order.DeliveryReminderSentAt = now;
            order.UpdatedAt = now;
            hasProcessedOrders = true;
        }

        if (hasProcessedOrders)
        {
            await unitOfWork.Commit();
        }
    }
}
