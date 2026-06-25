using Meisy.Communication.Responses.Notification;

namespace Meisy.Application.UseCases.Notifications.GetPreferences;

public interface IGetNotificationPreferencesUseCase
{
    Task<ResponseNotificationPreferencesJson> Execute(string endpoint);
}
