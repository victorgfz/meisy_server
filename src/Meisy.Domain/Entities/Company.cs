namespace Meisy.Domain.Entities
{
    public class Company : AuditableEntity
    {
        public string Code { get; set; } = string.Empty;
    }
}
