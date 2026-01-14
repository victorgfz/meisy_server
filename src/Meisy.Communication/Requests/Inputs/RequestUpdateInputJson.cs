namespace Meisy.Communication.Requests.Inputs
{
    public class RequestUpdateInputJson
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double Amount { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
