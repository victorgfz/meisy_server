using Meisy.Communication.Enums;

namespace Meisy.Communication.Responses.Products
{
    public class ResponseDetailedProductJson
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double Amount { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CompanyId { get; set; }

        public List<ResponseDetailedProductInputsJson> ProductInputs { get; set; } = [];
        public List<ResponseDetailedProductOverheadsJson> ProductOverheads { get; set; } = [];

    }
}
