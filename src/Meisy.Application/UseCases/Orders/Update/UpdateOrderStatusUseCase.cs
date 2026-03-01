using AutoMapper;
using Meisy.Communication.Requests.Orders;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Client;
using Meisy.Domain.Repositories.Order;
using Meisy.Domain.Repositories.Product;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Orders.Update;

public class UpdateOrderStatusUseCase : IUpdateOrderStatusUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IOrderReadOnlyRepository _orderReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderStatusUseCase(
        ILoggedUser loggedUser,
        IOrderReadOnlyRepository orderReadRepository,
        IUnitOfWork unitOfWork
        )
    {
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _orderReadRepository = orderReadRepository;
    }


    public async Task Execute(int id)
    {
        var companyId = _loggedUser.GetCompanyId();
        var order = await _orderReadRepository.GetByIdForUpdate(companyId, id) ?? throw new NotFoundException(ResourceErrorMessages.ORDER_NOT_FOUND);

        switch (order.Status)
        {
            case Domain.Enums.OrderStatus.Pending:
                order.Status = Domain.Enums.OrderStatus.Preparing;
                break;
            case Domain.Enums.OrderStatus.Preparing:
                order.Status = Domain.Enums.OrderStatus.Ready;
                break;
            case Domain.Enums.OrderStatus.Ready:
                order.Status = Domain.Enums.OrderStatus.Completed;
                break;
            case Domain.Enums.OrderStatus.Completed:
                throw new BusinessRuleException(ResourceErrorMessages.ORDER_STATUS_COMPLETED);
            case Domain.Enums.OrderStatus.Cancelled:
                throw new BusinessRuleException(ResourceErrorMessages.ORDER_STATUS_COMPLETED);
        }

        order.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Commit();
    }


}