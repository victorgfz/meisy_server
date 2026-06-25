namespace Meisy.Domain.Models;

public class PushNotificationPayload
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Url { get; set; } = "/dashboard/orders";
    public int? OrderId { get; set; }
}
