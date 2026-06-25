namespace Meisy.Communication.Requests.Notification
{
    public class RequestUpdatePreferencesJson
    {
        public string Endpoint { get; set; } = string.Empty;
        public bool ReceiveNotifications { get; set; }
    }
}
