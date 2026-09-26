using AutoMapper;
using Meisy.Application.Utils;
using Meisy.Communication.Responses;
using Meisy.Communication.Responses.Products;
using Meisy.Domain.Entities;
using Meisy.Domain.Repositories.Overhead;
using Meisy.Domain.Repositories.Product;
using Meisy.Domain.Services.LoggedUser;
using Meisy.Exception;
using Meisy.Exception.ExceptionBase;

namespace Meisy.Application.UseCases.Products.Get
{
    public class GetProductUseCase : IGetProductUseCase
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IProductReadOnlyRepository _productReadRepository;
        private readonly IOverheadReadOnlyRepository _overheadReadRepository;
        private readonly IMapper _mapper;

        public GetProductUseCase(
            ILoggedUser loggedUser,
            IProductReadOnlyRepository productReadRepository,
            IOverheadReadOnlyRepository overheadReadRepository,
            IMapper mapper
            )
        {
            _loggedUser = loggedUser;
            _productReadRepository = productReadRepository;
            _mapper = mapper;
            _overheadReadRepository = overheadReadRepository;
        }

        public async Task<ResponseDetailedProductJson> Execute(int id)
        {
            var companyId = _loggedUser.GetCompanyId();
            var product = await _productReadRepository.GetById(companyId, id) ?? throw new NotFoundException(ResourceErrorMessages.PRODUCT_NOT_FOUND);

            var entityProduct = _mapper.Map<ResponseDetailedProductJson>(product);
            AddProductInputs(product, entityProduct);

            var overheads = await _overheadReadRepository.GetAll(companyId);
            AddProductOverheads(overheads, entityProduct, (decimal)product.ProductionTime.TotalHours, product.Servings);

            return entityProduct;
        }

        private void AddProductInputs(Product product, ResponseDetailedProductJson entity)
        {
            foreach (var item in product.ProductInputs)
            {
                var formattedAmount = ProductCostUtils.FormatAmount(item.Input.Amount, (Communication.Enums.MeasurementUnit)item.Input.MeasurementUnit);
                var formattedProductionAmount = ProductCostUtils.FormatProductionAmount(item.ProductionAmount, (Communication.Enums.ProductionMeasurementUnit)item.ProductionMeasurementUnit);

                var productionPrice = formattedAmount > 0 && product.Servings > 0
                    ? (item.Input.Price / formattedAmount) * formattedProductionAmount / product.Servings
                    : 0;

                entity.ProductInputs.Add(new ResponseDetailedProductInputsJson
                {
                    Id = item.Input.Id,
                    Description = item.Input.Description,
                    Type = (Communication.Enums.InputType)item.Input.Type,
                    ProductionAmount = item.ProductionAmount,
                    ProductionMeasurementUnit = (Communication.Enums.ProductionMeasurementUnit)item.ProductionMeasurementUnit,
                    ProductionPrice = productionPrice
                });
            }
        }

        private void AddProductOverheads(List<Overhead> overheads, ResponseDetailedProductJson entity, decimal productionTime, int servings)
        {
            foreach (var item in overheads)
            {
                var totalCost = servings > 0
                    ? productionTime * item.CostPerHour / servings
                    : 0;

                entity.ProductOverheads.Add(new ResponseDetailedProductOverheadsJson
                {
                    Id = item.Id,
                    Type = (Communication.Enums.OverheadType)item.Type,
                    TotalCost = totalCost
                });
            }
        }
    }
}
