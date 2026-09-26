using Meisy.Domain.Entities;

namespace Meisy.Application.Utils
{
    public static class ProductCostUtils
    {
        public static decimal FormatAmount(double amount, Communication.Enums.MeasurementUnit unit)
        {
            var multiplier = unit switch
            {
                Communication.Enums.MeasurementUnit.kg => 1000m,
                Communication.Enums.MeasurementUnit.l => 1000m,
                _ => 1m
            };

            return (decimal)amount * multiplier;
        }

        public static decimal FormatProductionAmount(double amount, Communication.Enums.ProductionMeasurementUnit unit)
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

        public static decimal CalculateProductCost(Product product, List<Overhead> overheads)
        {
            // Custo por porção: custo total da receita dividido pela quantidade de porções que ela rende
            if (product.Servings <= 0) return 0;

            decimal productionPrice = 0;

            if (product.ProductInputs is not null)
            {
                foreach (var item in product.ProductInputs)
                {
                    if (item.Input is null) continue;

                    var formattedAmount = FormatAmount(item.Input.Amount, (Communication.Enums.MeasurementUnit)item.Input.MeasurementUnit);
                    if (formattedAmount == 0) continue;

                    var formattedProductionAmount = FormatProductionAmount(item.ProductionAmount, (Communication.Enums.ProductionMeasurementUnit)item.ProductionMeasurementUnit);

                    productionPrice += (item.Input.Price / formattedAmount) * formattedProductionAmount;
                }
            }

            if (overheads is not null)
            {
                foreach (var item in overheads)
                {
                    productionPrice += (decimal)product.ProductionTime.TotalHours * item.CostPerHour;
                }
            }

            return productionPrice / product.Servings;
        }
    }
}
