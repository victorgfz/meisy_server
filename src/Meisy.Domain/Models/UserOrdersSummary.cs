namespace Meisy.Domain.Models
{
    public class UserOrdersSummary
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int QuantityOfOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
