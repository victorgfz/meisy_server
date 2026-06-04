using Meisy.Communication.Responses.Reports;

namespace Meisy.Application.UseCases.Reports.GetAll
{
    public interface IGetAllReportUseCase
    {
        Task<ResponseReportsJson> Execute();

    }
}
