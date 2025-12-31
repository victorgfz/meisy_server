using Meisy.Domain.Enums;

namespace Meisy.Domain.Entities
{
    public class Product : AuditableEntity
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price {  get; set; }
        public double Amount { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }

        public TimeSpan ProductionTime { get; set; }
        public int Servings { get; set; }
        public List<ProductInput> ProdutInputs { get; set; } = [];


        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;
    }
}
