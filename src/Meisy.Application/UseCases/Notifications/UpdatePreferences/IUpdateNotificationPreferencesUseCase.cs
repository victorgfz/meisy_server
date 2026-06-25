using Meisy.Communication.Requests.Notification;

namespace Meisy.Application.UseCases.Notifications.UpdatePreferences;

public interface IUpdateNotificationPreferencesUseCase
{
    Task Execute(RequestUpdatePreferencesJson request);
}
