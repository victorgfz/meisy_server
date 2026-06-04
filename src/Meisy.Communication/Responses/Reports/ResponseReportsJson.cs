namespace Meisy.Communication.Responses.Reports
{
    public class ResponseReportsJson
    {
        public ResponseReportCurrentMonthJson CurrentMonth { get; set; } = default!;
        public List<ResponseReportPreviousMonthJson> PreviousMonths { get; set; } = [];
    }
}
