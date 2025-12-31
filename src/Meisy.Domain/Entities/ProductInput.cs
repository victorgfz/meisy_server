using Meisy.Domain.Enums;

namespace Meisy.Domain.Entities
{
    public class ProductInput
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public int InputId { get; set; }
        public Input Input { get; set; } = default!;

        public double ProductionAmount { get; set; }
        public ProductionMeasurementUnit ProductionMeasurementUnit { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;


    }
}
