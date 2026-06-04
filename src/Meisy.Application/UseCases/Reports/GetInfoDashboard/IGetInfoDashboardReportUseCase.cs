using Meisy.Communication.Responses.Reports;

namespace Meisy.Application.UseCases.Reports.GetInfoDashboard
{
    public  interface IGetInfoDashboardReportUseCase
    {
        Task<ResponseReportInfoDashboardJson> Execute();

    }
}
