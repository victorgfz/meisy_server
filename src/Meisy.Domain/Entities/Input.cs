using Meisy.Domain.Enums;

namespace Meisy.Domain.Entities
{
    public class Input : AuditableEntity
    {
       
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public InputType Type { get; set; }
        public double Amount { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;
    }
}
