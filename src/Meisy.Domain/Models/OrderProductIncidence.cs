namespace Meisy.Domain.Models
{
    public class OrderProductIncidence
    {
        public int ProductId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Total { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
