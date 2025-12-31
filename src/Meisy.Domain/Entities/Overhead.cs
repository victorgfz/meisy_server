using Meisy.Domain.Enums;

namespace Meisy.Domain.Entities
{
    public class Overhead : AuditableEntity
    {
        public OverheadType Type { get; set; }
        public decimal CostPerHour {  get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;
    }
}
