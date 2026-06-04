using Meisy.Communication.Responses.Reports;
using Meisy.Domain.Repositories.Order;
using Meisy.Domain.Services.LoggedUser;
using System.Collections.Generic;

namespace Meisy.Application.UseCases.Reports.GetAll
{
    public class GetAllReportUseCase : IGetAllReportUseCase
    {


        private readonly ILoggedUser _loggedUser;
        private readonly IOrderReadOnlyRepository _orderReadRepository;

        public GetAllReportUseCase(ILoggedUser loggedUser, IOrderReadOnlyRepository orderReadRepository)
        {
            _loggedUser = loggedUser;
            _orderReadRepository = orderReadRepository;
        }

        public async Task<ResponseReportsJson> Execute()
        {

            var companyId = _loggedUser.GetCompanyId();

            var currentMonth = DateTime.UtcNow;
            var ordersCurrentMonth = await _orderReadRepository.GetAllByMonth(companyId, currentMonth);
            var quantityOfOrders = ordersCurrentMonth.Count;
            var quantityOfCompletedOrders = 0;
            decimal totalRevenue = 0;
            decimal totalCosts = 0;

            if (ordersCurrentMonth.Count > 0)
            {
                quantityOfCompletedOrders = ordersCurrentMonth.Count(o => o.Status == Domain.Enums.OrderStatus.Completed);
                totalRevenue = ordersCurrentMonth.Where(o => o.Status == Domain.Enums.OrderStatus.Completed).Sum(o => o.TotalPrice);
                totalCosts = ordersCurrentMonth.Where(o => o.Status == Domain.Enums.OrderStatus.Completed).SelectMany(o => o.OrderProducts)
                    .Sum(p => p.CostAtTheMoment);

            }
            
            decimal quantityOfOrdersVariationRate = 0;
            decimal totalCostsVariationRate = 0;
            decimal totalRevenueVariationRate = 0;
            decimal totalProfitVariationRate = 0;

            decimal totalCostsPrevious = 0;
            decimal totalRevenuePrevious = 0;
            List<ResponseReportPreviousMonthJson> previousMonths = [];

            for (var i = 0; i < 4; i++)
            {

                var monthsToDecrease = -1 - i;
                   
                var month = DateTime.UtcNow.AddMonths(monthsToDecrease);
                var ordersMonth = await _orderReadRepository.GetAllByMonth(companyId, month);
                var quantityOfOrdersMonth = ordersMonth.Count;
                var quantityOfCompletedOrdersMonth = 0;
                decimal totalRevenueMonth = 0;
                decimal totalCostsMonth = 0;
                if (ordersMonth.Count > 0)
                {
                    quantityOfCompletedOrdersMonth = ordersMonth.Count(o => o.Status == Domain.Enums.OrderStatus.Completed);
                    totalRevenueMonth = ordersMonth.Where(o => o.Status == Domain.Enums.OrderStatus.Completed).Sum(o => o.TotalPrice);
                    totalCostsMonth = ordersMonth.Where(o => o.Status == Domain.Enums.OrderStatus.Completed).SelectMany(o => o.OrderProducts).Sum(p => p.CostAtTheMoment);

                    if(i == 0)
                    {
                        quantityOfOrdersVariationRate = quantityOfOrdersMonth > 0 ? (decimal)quantityOfOrders / (decimal)quantityOfOrdersMonth : 0;
                        totalCostsVariationRate = totalCostsMonth > 0 ? totalCosts / totalCostsMonth : 0;
                        totalRevenueVariationRate = totalRevenueMonth > 0 ? totalRevenue / totalRevenueMonth : 0;
                        totalProfitVariationRate = totalRevenueMonth > 0 && totalCostsMonth > 0 ? (totalRevenue - totalCosts) / (totalRevenueMonth - totalCostsMonth) : 0;
                    } else
                    {
                        previousMonths[i-1].TotalCostsVariationRate = totalCostsMonth > 0 ? totalCostsPrevious / totalCostsMonth : 0;
                        previousMonths[i-1].TotalRevenueVariationRate = totalRevenueMonth > 0 ? totalRevenuePrevious / totalRevenueMonth : 0;
                        previousMonths[i-1].TotalProfitVariationRate = totalRevenueMonth > 0 && totalCostsMonth > 0 ? (totalRevenuePrevious - totalCostsPrevious) /(totalRevenueMonth - totalCostsMonth)  : 0;
                    }
                       
                }
                if(i < 3)
                {
                    previousMonths.Add(new ResponseReportPreviousMonthJson
                    {
                        QuantityOfOrders = quantityOfOrdersMonth,
                        QuantityOfCompletedOrders = quantityOfCompletedOrdersMonth,
                        TotalCosts = totalCostsMonth,
                        TotalProfit = totalRevenueMonth - totalCostsMonth,
                        TotalRevenue = totalRevenueMonth,
                        TotalCostsVariationRate = 0,
                        TotalProfitVariationRate = 0,
                        TotalRevenueVariationRate = 0,
                    });
                }
                totalCostsPrevious = totalCostsMonth;
                totalRevenuePrevious = totalRevenueMonth;
            }


            var topProducts = await _orderReadRepository.GetTopProductsByMonth(companyId, currentMonth, 3);
            List<ResponseBestSellingProductJson> bestSellingProducts = topProducts
                .Select(x => new ResponseBestSellingProductJson
                {
                    Description = x.Description,
                    Total = x.Total,
                    TotalRevenue = x.TotalRevenue
                })
                .ToList();

            return new ResponseReportsJson
            {
                CurrentMonth = new ResponseReportCurrentMonthJson
                {
                    QuantityOfOrders = quantityOfOrders,
                    QuantityOfCompletedOrders = quantityOfCompletedOrders,

                    TotalCosts = totalCosts,
                    TotalProfit = totalRevenue - totalCosts,
                    TotalRevenue = totalRevenue,

                    QuantityOfOrdersVariationRate = quantityOfOrdersVariationRate,
                    TotalCostsVariationRate = totalCostsVariationRate,
                    TotalProfitVariationRate = totalProfitVariationRate,
                    TotalRevenueVariationRate = totalRevenueVariationRate,

                    BestSellingProducts = bestSellingProducts
                },
                PreviousMonths = previousMonths
            };


            throw new NotImplementedException();
        }
    }
}
