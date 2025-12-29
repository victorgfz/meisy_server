using Meisy.Communication.Enums;

namespace Meisy.Communication.Requests
{
    public class RequestInputJson
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public InputType Type { get; set; }
        public int Amount { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
