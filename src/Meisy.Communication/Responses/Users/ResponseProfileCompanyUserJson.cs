namespace Meisy.Communication.Responses.Users
{
    public class ResponseProfileCompanyUserJson
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int QuantityOfOrders { get; set; }
        public bool IsLoggedUser { get; set; }
    }
}
