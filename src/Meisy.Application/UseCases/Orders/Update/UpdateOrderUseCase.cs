using Meisy.Application.Utils;
using Meisy.Communication.Requests.Orders;
using Meisy.Domain.Entities;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Order;
using Meisy.Domain.Repositories.Overhead;
using Meisy.Domain.Repositories.Product;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Orders.Update
{
    public class UpdateOrderUseCase : IUpdateOrderUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IOrderReadOnlyRepository _orderReadRepository;
        private readonly IOrderWriteOnlyRepository _orderWriteRepository;
        private readonly IProductReadOnlyRepository _productReadRepository;
        private readonly IOverheadReadOnlyRepository _overheadReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderUseCase(
            ILoggedUser loggedUser,
            IOrderReadOnlyRepository orderReadRepository,
            IOrderWriteOnlyRepository orderWriteRepository,
            IProductReadOnlyRepository productReadRepository,
            IOverheadReadOnlyRepository overheadReadRepository,
            IUnitOfWork unitOfWork)
        {
            _loggedUser = loggedUser;
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _productReadRepository = productReadRepository;
            _overheadReadRepository = overheadReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(RequestUpdateOrderJson request, int id)
        {
            var companyId = _loggedUser.GetCompanyId();
            Validate(request);

            var order = await _orderReadRepository.GetByIdForUpdate(companyId, id);
            if (order is null)
            {
                throw new NotFoundException(ResourceErrorMessages.ORDER_NOT_FOUND);
            }

            if (order.Status == Domain.Enums.OrderStatus.Completed || order.Status == Domain.Enums.OrderStatus.Cancelled)
            {
                throw new BusinessRuleException(ResourceErrorMessages.ORDER_STATUS_COMPLETED);
            }

            if (order.DeliveryDate != request.DeliveryDate)
            {
                var today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "E. South America Standard Time").Date;
                var requestDate = request.DeliveryDate.Date;

                if (requestDate < today && request.DeliveryDate.Date < DateTime.UtcNow.Date)
                {
                    throw new ErrorOnValidationException([ResourceErrorMessages.INVALID_DATE]);
                }

                order.DeliveryDate = request.DeliveryDate;
                order.DeliveryReminderSentAt = null;
            }

            var overheads = await _overheadReadRepository.GetAll(companyId);

            // Identify items to remove
            var itemsToRemove = order.OrderProducts
                .Where(op => !request.OrderProducts.Any(req => req.ProductId == op.ProductId))
                .ToList();

            if (itemsToRemove.Count > 0)
            {
                _orderWriteRepository.DeleteOrderProducts(itemsToRemove);
                order.OrderProducts.RemoveAll(op => itemsToRemove.Contains(op));
            }

            foreach (var item in request.OrderProducts)
            {
                var product = await _productReadRepository.GetById(companyId, item.ProductId)
                    ?? throw new NotFoundException(ResourceErrorMessages.PRODUCT_NOT_FOUND);

                var existingItem = order.OrderProducts.FirstOrDefault(op => op.ProductId == item.ProductId);
                if (existingItem is not null)
                {
                    existingItem.Amount = item.Amount;
                    existingItem.PriceAtTheMoment = product.Price;
                    existingItem.CostAtTheMoment = ProductCostUtils.CalculateProductCost(product, overheads);
                }
                else
                {
                    order.OrderProducts.Add(new OrderProduct
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Amount = item.Amount,
                        PriceAtTheMoment = product.Price,
                        CostAtTheMoment = ProductCostUtils.CalculateProductCost(product, overheads),
                        CompanyId = companyId
                    });
                }
            }

            order.TotalPrice = order.OrderProducts.Sum(op => op.PriceAtTheMoment * op.Amount);
            order.UpdatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "E. South America Standard Time");

            await _unitOfWork.Commit();
        }

        private void Validate(RequestUpdateOrderJson request)
        {
            var result = new UpdateOrderValidator().Validate(request);
            if (!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
