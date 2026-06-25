using Meisy.Communication.Responses.Notification;
using Meisy.Domain.Repositories.PushSubscription;
using Meisy.Domain.Services.LoggedUser;

namespace Meisy.Application.UseCases.Notifications.GetPreferences;

public class GetNotificationPreferencesUseCase : IGetNotificationPreferencesUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IPushSubscriptionReadOnlyRepository _pushSubscriptionReadRepository;

    public GetNotificationPreferencesUseCase(
        ILoggedUser loggedUser,
        IPushSubscriptionReadOnlyRepository pushSubscriptionReadRepository)
    {
        _loggedUser = loggedUser;
        _pushSubscriptionReadRepository = pushSubscriptionReadRepository;
    }

    public async Task<ResponseNotificationPreferencesJson> Execute(string endpoint)
    {
        var userId = _loggedUser.GetUserId();
        var subscription = await _pushSubscriptionReadRepository.GetByEndpoint(endpoint);

        if (subscription is null || subscription.UserId != userId)
        {
            return new ResponseNotificationPreferencesJson { ReceiveNotifications = false };
        }

        return new ResponseNotificationPreferencesJson
        {
            ReceiveNotifications = subscription.ReceiveNotifications,
        };
    }
}
