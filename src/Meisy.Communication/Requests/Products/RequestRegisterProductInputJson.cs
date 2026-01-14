using Meisy.Communication.Enums;

namespace Meisy.Communication.Requests.Products
{
    public class RequestRegisterProductInputJson
    {
        public int InputId { get; set; }
        public double ProductionAmount { get; set; }
        public ProductionMeasurementUnit ProductionMeasurementUnit { get; set; }
    }
}
