namespace Meisy.Communication.Requests.Orders
{
    public class RequestUpdateOrderJson
    {
        public DateTime DeliveryDate { get; set; }
        public List<RequestRegisterOrderProductJson> OrderProducts { get; set; } = [];
        public DateTime UpdatedAt { get; set; }
    }
}
