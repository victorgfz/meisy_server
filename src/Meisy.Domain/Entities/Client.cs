namespace Meisy.Domain.Entities
{
    public class Client : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;
    }
}
