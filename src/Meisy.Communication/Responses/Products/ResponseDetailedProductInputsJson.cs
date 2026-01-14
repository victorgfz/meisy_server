using Meisy.Communication.Enums;

namespace Meisy.Communication.Responses.Products
{
    public class ResponseDetailedProductInputsJson
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty; 
        public InputType Type { get; set; }
        public double ProductionAmount { get; set; }
        public ProductionMeasurementUnit ProductionMeasurementUnit { get; set; }
        public decimal ProductionPrice { get; set; }

    }
}
