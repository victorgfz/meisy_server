using AutoMapper;
using Meisy.Communication.Responses;
using Meisy.Communication.Responses.Products;
using Meisy.Domain.Entities;
using Meisy.Domain.Enums;
using Meisy.Domain.Repositories.Overhead;
using Meisy.Domain.Repositories.Products;
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

            var overheads =  await _overheadReadRepository.GetAll(companyId);
            AddProductOverheads(overheads, entityProduct, (decimal)product.ProductionTime.TotalHours);
           
            return entityProduct;
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

        private static decimal FormatProductionAmount( double amount, Communication.Enums.ProductionMeasurementUnit unit)
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

        private void AddProductInputs(Product product, ResponseDetailedProductJson entity)
        {
            foreach(var item in product.ProductInputs)
            {
                var productionPrice =
                    item.Input.Price /
                    FormatAmount(item.Input.Amount, (Communication.Enums.MeasurementUnit)item.Input.MeasurementUnit)*
                    FormatProductionAmount(item.ProductionAmount, (Communication.Enums.ProductionMeasurementUnit)item.ProductionMeasurementUnit);

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

        private void AddProductOverheads(List<Overhead> overheads, ResponseDetailedProductJson entity, decimal productionTime)
        {
            foreach(var item in overheads)
            {
                entity.ProductOverheads.Add(new ResponseDetailedProductOverheadsJson
                {
                    Id = item.Id,
                    Type = (Communication.Enums.OverheadType)item.Type,
                    TotalCost = productionTime * item.CostPerHour
                });
            }
        }
    }
}
