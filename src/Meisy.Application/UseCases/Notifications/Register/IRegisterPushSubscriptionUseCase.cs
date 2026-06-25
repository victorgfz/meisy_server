using Meisy.Communication.Requests.Notification;

namespace Meisy.Application.UseCases.Notifications.Register;

public interface IRegisterPushSubscriptionUseCase
{
    Task Execute(RequestPushSubscriptionJson request);
}
