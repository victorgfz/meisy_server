namespace Meisy.Communication.Requests.Orders
{
    public class RequestRegisterOrderJson
    {
        public DateTime DeliveryDate { get; set; }
        public int ClientId { get; set; }
        public List<RequestRegisterOrderProductJson> OrderProducts { get; set; } = [];

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
