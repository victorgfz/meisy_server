namespace Meisy.Domain.Entities
{
    public class AuditableEntity : Entity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
