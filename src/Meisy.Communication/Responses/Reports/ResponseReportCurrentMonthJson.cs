namespace Meisy.Communication.Responses.Reports
{
    public class ResponseReportCurrentMonthJson
    {
        public int QuantityOfOrders { get; set; }
        public decimal QuantityOfOrdersVariationRate { get; set; }
        public int QuantityOfCompletedOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalRevenueVariationRate { get; set; }
        public decimal TotalCosts   { get; set; }
        public decimal TotalCostsVariationRate { get; set; }

        public decimal TotalProfit  { get; set; }
        public decimal TotalProfitVariationRate { get; set; }

        public List<ResponseBestSellingProductJson> BestSellingProducts { get; set; } = [];

    }
}
