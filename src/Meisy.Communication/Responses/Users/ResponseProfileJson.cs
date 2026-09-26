namespace Meisy.Communication.Responses.Users
{
    public class ResponseProfileJson
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CompanyCode { get; set; } = string.Empty;
        public int QuantityOfOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<ResponseProfileCompanyUserJson> CompanyUsers { get; set; } = [];
    }
}
