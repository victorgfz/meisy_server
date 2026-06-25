using Meisy.Communication.Requests.Notification;
using Meisy.Domain.Entities;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.PushSubscription;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Notifications.Register;

public class RegisterPushSubscriptionUseCase : IRegisterPushSubscriptionUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IPushSubscriptionReadOnlyRepository _pushSubscriptionReadRepository;
    private readonly IPushSubscriptionWriteOnlyRepository _pushSubscriptionWriteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterPushSubscriptionUseCase(
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

    public async Task Execute(RequestPushSubscriptionJson request)
    {
        Validate(request);

        var userId = _loggedUser.GetUserId();
        var now = DateTime.UtcNow;
        var subscription = await _pushSubscriptionReadRepository.GetByEndpoint(request.Endpoint);

        if (subscription is null)
        {
            await _pushSubscriptionWriteRepository.Add(new PushSubscription
            {
                Endpoint = request.Endpoint,
                P256DH = request.P256DH,
                Auth = request.Auth,
                UserId = userId,
                ReceiveNotifications = true,
                LastUsedAt = now,
                CreatedAt = now,
                UpdatedAt = now,
            });
        }
        else
        {
            subscription.P256DH = request.P256DH;
            subscription.Auth = request.Auth;
            subscription.UserId = userId;
            subscription.ReceiveNotifications = true;
            subscription.LastUsedAt = now;
            subscription.UpdatedAt = now;

            _pushSubscriptionWriteRepository.Update(subscription);
        }

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestPushSubscriptionJson request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Endpoint)) errors.Add("Endpoint da assinatura e obrigatorio.");
        if (string.IsNullOrWhiteSpace(request.P256DH)) errors.Add("Chave p256dh da assinatura e obrigatoria.");
        if (string.IsNullOrWhiteSpace(request.Auth)) errors.Add("Chave auth da assinatura e obrigatoria.");

        if (errors.Count != 0)
        {
            throw new ErrorOnValidationException(errors);
        }
    }
}
