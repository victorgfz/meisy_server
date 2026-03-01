namespace Meisy.Communication.Responses.Clients
{
    public class ResponseClientJson
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public int CompanyId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
