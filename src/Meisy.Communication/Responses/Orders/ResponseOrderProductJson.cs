namespace Meisy.Communication.Responses.Orders
{
    public class ResponseOrderProductJson
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Amount { get; set; }
        public decimal PriceAtTheMoment { get; set; }
    }
}
