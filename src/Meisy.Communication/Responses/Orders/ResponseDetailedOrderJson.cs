using Meisy.Communication.Enums;

namespace Meisy.Communication.Responses.Orders
{
    public class ResponseDetailedOrderJson
    {
        public int Id { get; set; }
        public DateTime DeliveryDate { get; set; }

        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }

        public ResponseOrderUserJson Seller { get; set; } = default!;

        public ResponseOrderUserJson Client { get; set; } = default!;

        public List<ResponseOrderProductJson> OrderProducts { get; set; } = [];

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public int CompanyId { get; set; }
    }
}
