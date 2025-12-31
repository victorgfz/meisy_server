using Meisy.Communication.Enums;

namespace Meisy.Communication.Requests
{
    public class RequestRegisterInputJson
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public InputType Type { get; set; }
        public double Amount { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
