using Meisy.Communication.Enums;

namespace Meisy.Communication.Responses.Orders
{
    public class ResponseOrderJson
    {
        public int Id { get; set; }
        public DateTime DeliveryDate { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public int CompanyId { get; set; }
    }
}
