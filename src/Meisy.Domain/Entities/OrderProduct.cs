namespace Meisy.Domain.Entities
{
    public class OrderProduct
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;

        public int Amount { get; set; }
        public decimal PriceAtTheMoment { get; set; }


        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;


    }
}
