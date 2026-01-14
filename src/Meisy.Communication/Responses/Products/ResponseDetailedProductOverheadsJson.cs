using Meisy.Communication.Enums;

namespace Meisy.Communication.Responses.Products
{
    public class ResponseDetailedProductOverheadsJson
    {
        public int Id { get; set; }
        public OverheadType Type { get; set; }
        public decimal TotalCost { get; set; }
    }
}
