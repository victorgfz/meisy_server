using Meisy.Communication.Requests.Notification;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.PushSubscription;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Notifications.UpdatePreferences;

public class UpdateNotificationPreferencesUseCase : IUpdateNotificationPreferencesUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IPushSubscriptionReadOnlyRepository _pushSubscriptionReadRepository;
    private readonly IPushSubscriptionWriteOnlyRepository _pushSubscriptionWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateNotificationPreferencesUseCase(
        ILoggedUser loggedUser,
        IPushSubscriptionReadOnlyRepository pushSubscriptionReadRepository,
        IPushSubscriptionWriteOnlyRepository pushSubscriptionWriteRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _pushSubscriptionReadRepository = pushSubscriptionReadRepository;
        _pushSubscriptionWriteRepository = pushSubscriptionWriteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestUpdatePreferencesJson request)
    {
        if (string.IsNullOrWhiteSpace(request.Endpoint))
        {
            throw new ErrorOnValidationException(["Endpoint da assinatura e obrigatorio."]);
        }

        var userId = _loggedUser.GetUserId();
        var subscription = await _pushSubscriptionReadRepository.GetByEndpoint(request.Endpoint);

        if (subscription is null || subscription.UserId != userId)
        {
            return;
        }

        subscription.ReceiveNotifications = request.ReceiveNotifications;
        subscription.LastUsedAt = DateTime.UtcNow;
        subscription.UpdatedAt = DateTime.UtcNow;

        _pushSubscriptionWriteRepository.Update(subscription);
        await _unitOfWork.Commit();
    }
}
