namespace Meisy.Communication.Requests.Products
{
    public class RequestUpdateProductJson
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double Amount { get; set; }
        public TimeSpan ProductionTime { get; set; }
        public int Servings { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<RequestRegisterProductInputJson> ProductInputs { get; set; } = [];
    }
}
