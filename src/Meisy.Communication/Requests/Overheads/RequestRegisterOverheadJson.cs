using Meisy.Communication.Enums;

namespace Meisy.Communication.Requests.Overheads
{
    public class RequestRegisterOverheadJson
    {
        public OverheadType Type { get; set; }
        public decimal CostPerHour { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
    }
}
