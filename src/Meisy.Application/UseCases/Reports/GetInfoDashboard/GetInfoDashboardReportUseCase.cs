using AutoMapper;
using Meisy.Communication.Responses.Reports;
using Meisy.Domain.Repositories;
using Meisy.Domain.Repositories.Client;
using Meisy.Domain.Repositories.Order;
using Meisy.Domain.Services.LoggedUser;

namespace Meisy.Application.UseCases.Reports.GetInfoDashboard
{
    public class GetInfoDashboardReportUseCase : IGetInfoDashboardReportUseCase
    {

        private readonly ILoggedUser _loggedUser;
        private readonly IOrderReadOnlyRepository _orderReadRepository;

        public GetInfoDashboardReportUseCase(ILoggedUser loggedUser, IOrderReadOnlyRepository orderReadRepository)
        {
            _loggedUser = loggedUser;
            _orderReadRepository= orderReadRepository;
        }

        public async Task<ResponseReportInfoDashboardJson> Execute()
        {
            var companyId = _loggedUser.GetCompanyId();

            var currentMonth = DateTime.UtcNow;
            var lastMonth = DateTime.UtcNow.AddMonths(-1);

            var quantityOfOrdersCurrentMonth = await _orderReadRepository.GetAllByMonth(companyId, currentMonth);
            var quantityOfOrdersLastMonth = await _orderReadRepository.GetAllByMonth(companyId, lastMonth);

            decimal balanceCurrentMonth = quantityOfOrdersCurrentMonth.Count > 0 ? quantityOfOrdersCurrentMonth.Where(o => o.Status == Domain.Enums.OrderStatus.Completed).Sum(o => o.TotalPrice) : 0;
            decimal balanceLastMonth = quantityOfOrdersLastMonth.Count > 0 ? quantityOfOrdersLastMonth.Where(o => o.Status == Domain.Enums.OrderStatus.Completed).Sum(o => o.TotalPrice) : 0;


            var variationRate = balanceLastMonth == 0 ? 0 : balanceCurrentMonth / balanceLastMonth;
            var quantityOfCompletedOrdersCurrentMonth = quantityOfOrdersCurrentMonth.Count > 0 ? quantityOfOrdersCurrentMonth.Count(o => o.Status == Domain.Enums.OrderStatus.Completed) : 0;

            return new ResponseReportInfoDashboardJson
            {
                Balance = balanceCurrentMonth,
                VariationRate = variationRate,
                QuantityOfOrders = quantityOfOrdersCurrentMonth.Count,
                QuantityOfCompletedOrders = quantityOfCompletedOrdersCurrentMonth,
            };
        }
    }
}
