using Meisy.Communication.Enums;

namespace Meisy.Communication.Responses
{
    public class ResponseInputJson
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public InputType Type { get; set; }
        public double Amount { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CompanyId { get; set; }
    }
}
