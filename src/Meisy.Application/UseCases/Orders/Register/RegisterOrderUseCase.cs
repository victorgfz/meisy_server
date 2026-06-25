using AutoMapper;
using Meisy.Communication.Requests.Orders;
using Meisy.Application.Services.Notifications;
using Meisy.Communication.Responses.Orders;
using Meisy.Communication.Responses.Products;
using Meisy.Domain.Entities;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Client;
using Meisy.Domain.Repositories.Input;
using Meisy.Domain.Repositories.Order;
using Meisy.Domain.Repositories.Overhead;
using Meisy.Domain.Repositories.Product;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;
using Microsoft.VisualBasic;

namespace Meisy.Application.UseCases.Orders.Register
{
    public class RegisterOrderUseCase : IRegisterOrderUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IProductReadOnlyRepository _productReadRepository;
        private readonly IOrderWriteOnlyRepository _orderWriteRepository;
        private readonly IClientReadOnlyRepository _clientReadRepository;
        private readonly IOverheadReadOnlyRepository _overheadReadRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICompanyNotificationService _companyNotificationService;

        public RegisterOrderUseCase(
            ILoggedUser loggedUser,
            IProductReadOnlyRepository productReadRepository,
            IOrderWriteOnlyRepository orderWriteRepository,
            IClientReadOnlyRepository clientReadRepository,
            IOverheadReadOnlyRepository overheadReadRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ICompanyNotificationService companyNotificationService
            )
        {
            _loggedUser = loggedUser;
            _productReadRepository = productReadRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _orderWriteRepository = orderWriteRepository;
            _clientReadRepository = clientReadRepository;
            _overheadReadRepository = overheadReadRepository;

            _companyNotificationService = companyNotificationService;
        }

        public async Task<ResponseOrderJson> Execute(RequestRegisterOrderJson request)
        {
            var companyId = _loggedUser.GetCompanyId();
            var userId = _loggedUser.GetUserId();
            Validate(request);

            var clientId = request.ClientId;

            var client = clientId is null ? null : (await _clientReadRepository.GetById(companyId, clientId.Value)
                         ?? throw new NotFoundException(ResourceErrorMessages.CLIENT_NOT_FOUND)) ;

            var entityOrder = _mapper.Map<Order>(request);


            entityOrder.CompanyId = companyId;
            entityOrder.SellerId = userId;
            entityOrder.Status = Domain.Enums.OrderStatus.Pending;

            entityOrder.OrderProducts = _mapper.Map<List<OrderProduct>>(request.OrderProducts);
            var overheads = await _overheadReadRepository.GetAll(companyId);

            foreach (var item in entityOrder.OrderProducts)
            {
                var product = await _productReadRepository.GetById(companyId, item.ProductId) ?? throw new NotFoundException(ResourceErrorMessages.PRODUCT_NOT_FOUND);
                item.PriceAtTheMoment = product.Price;

                item.CostAtTheMoment = CalculateProductCost(product, overheads);
                entityOrder.TotalPrice = item.PriceAtTheMoment * item.Amount + entityOrder.TotalPrice;
                item.CompanyId = companyId;
                
            }

            await _orderWriteRepository.Add(entityOrder);
            await _unitOfWork.Commit();

            try
            {
                await _companyNotificationService.NotifyOrderCreated(companyId, entityOrder.Id, entityOrder.DeliveryDate);
            }
            catch
            {
                // Push notification failures must not make order creation fail.
            }

            return _mapper.Map<ResponseOrderJson>(entityOrder);

        }


        private static decimal FormatAmount(double amount, Communication.Enums.MeasurementUnit unit)
        {
            var multiplier = unit switch
            {
                Communication.Enums.MeasurementUnit.kg => 1000m,
                Communication.Enums.MeasurementUnit.l => 1000m,
                _ => 1m
            };

            return (decimal)amount * multiplier;
        }

        private static decimal FormatProductionAmount(double amount, Communication.Enums.ProductionMeasurementUnit unit)
        {
            var multiplier = unit switch
            {
                Communication.Enums.ProductionMeasurementUnit.kg => 1000m,
                Communication.Enums.ProductionMeasurementUnit.l => 1000m,
                Communication.Enums.ProductionMeasurementUnit.tsp => 5m,
                Communication.Enums.ProductionMeasurementUnit.tbscp => 15m,
                _ => 1m
            };

            return (decimal)amount * multiplier;
        }

        private decimal CalculateProductCost(Product product, List<Overhead> overheads)
        {
            decimal productionPrice = 0;
            foreach (var item in product.ProductInputs)
            {
                productionPrice =
                    productionPrice +
                    (item.Input.Price /
                    FormatAmount(item.Input.Amount, (Communication.Enums.MeasurementUnit)item.Input.MeasurementUnit) *
                    FormatProductionAmount(item.ProductionAmount, (Communication.Enums.ProductionMeasurementUnit)item.ProductionMeasurementUnit));
            }
            foreach (var item in overheads)
            {
                productionPrice = productionPrice + (
                    (decimal)product.ProductionTime.TotalHours * item.CostPerHour / product.Servings
                    );
            }
            return productionPrice;
        }

        private void Validate(RequestRegisterOrderJson request)
        {
            var result = new RegisterOrderValidator().Validate(request);
            if(!result.IsValid)
            {
                var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
                throw new ErrorOnValidationException(errors);
            }
        }
    }
}
