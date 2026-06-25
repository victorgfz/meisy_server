namespace Meisy.Domain.Entities
{
    public class User : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public Company Company { get; set; } = default!;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public List<PushSubscription> PushSubscriptions { get; set; } = [];
        
    }
}
