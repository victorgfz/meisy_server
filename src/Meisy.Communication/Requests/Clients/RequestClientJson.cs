namespace Meisy.Communication.Requests.Clients
{
    public class RequestClientJson
    {
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
