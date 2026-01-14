using Meisy.Communication.Enums;

namespace Meisy.Communication.Responses.Overheads
{
    public class ResponseOverheadJson
    {
        public int Id { get; set; }
        public OverheadType Type { get; set; }
        public decimal CostPerHour { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CompanyId { get; set; }
    }
}
