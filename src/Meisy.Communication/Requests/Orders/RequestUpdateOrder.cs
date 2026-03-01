using Meisy.Communication.Enums;

namespace Meisy.Communication.Requests.Orders;

public class RequestUpdateOrder
{
    public OrderStatus CurrentStatus { get; set; }
    public DateTime UpdatedAt   { get; set; }
}
