namespace Meisy.Domain.Entities
{
    public class PushSubscription : AuditableEntity
    {
        public string Endpoint { get; set; } = string.Empty;
        public string P256DH { get; set; } = string.Empty;
        public string Auth { get; set; } = string.Empty;
        public bool ReceiveNotifications { get; set; } = true;
        public DateTime? LastUsedAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
