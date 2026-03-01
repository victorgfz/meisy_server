using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Order;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Orders.Update
{
    public class CancelOrderStatusUseCase : ICancelOrderStatusUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IOrderReadOnlyRepository _orderReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelOrderStatusUseCase(
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
                case Domain.Enums.OrderStatus.Completed:
                    throw new BusinessRuleException(ResourceErrorMessages.ORDER_STATUS_COMPLETED);
                case Domain.Enums.OrderStatus.Cancelled:
                    throw new BusinessRuleException(ResourceErrorMessages.ORDER_STATUS_COMPLETED);
            }

            order.Status = Domain.Enums.OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Commit();
        }
    }
}
