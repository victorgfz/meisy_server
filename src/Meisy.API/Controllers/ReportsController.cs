using Meisy.Application.UseCases.Products.GetAll;
using Meisy.Application.UseCases.Reports.GetAll;
using Meisy.Application.UseCases.Reports.GetInfoDashboard;
using Meisy.Communication.Responses.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meisy.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [Authorize]

    public class ReportsController : ControllerBase
    {
        [HttpGet("info-dashboard")]
        [ProducesResponseType(typeof(ResponseReportInfoDashboardJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInfoDashboard([FromServices] IGetInfoDashboardReportUseCase useCase)
        {
            var result = await useCase.Execute();

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseReportsJson), StatusCodes.Status200OK)]

        public async Task<IActionResult> GetAll ([FromServices] IGetAllReportUseCase useCase)
        {
            var result = await useCase.Execute();
            return Ok(result);
        }
    }
}
