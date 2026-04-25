using Meisy.Domain.Enums;

namespace Meisy.Domain.Entities
{
    public class Order : AuditableEntity
    {
        public decimal TotalPrice { get; set; }
        public DateTime DeliveryDate { get; set; }

        public OrderStatus Status { get; set; }


        public int SellerId { get; set; }
        public User Seller { get; set; } = default!;

        public int? ClientId { get; set; }
        public Client? Client { get; set; } = default!;

        public List<OrderProduct> OrderProducts { get; set; } = [];
    

        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;
    }
}
