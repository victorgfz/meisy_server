using Meisy.Application.UseCases.Overheads.GetAll;
using Meisy.Application.UseCases.Overheads.Register;
using Meisy.Application.UseCases.Overheads.Update;
using Meisy.Communication.Requests.Overheads;
using Meisy.Communication.Responses;
using Meisy.Communication.Responses.Overheads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meisy.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [Authorize]
    public class OverheadsController : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(typeof(ResponseOverheadJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register(
            [FromBody] RequestRegisterOverheadJson request,
            [FromServices] IRegisterOverheadUseCase useCase
            )
        {
            var response = await useCase.Execute(request);
            return Created(string.Empty, response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ResponseOverheadJson>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllOverheadUseCase useCase
            )
        {
            var response = await useCase.Execute();
            if (response.Count != 0) return Ok(response);

            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromServices] IUpdateOverheadUseCase useCase,
            [FromBody] RequestUpdateOverheadJson request
            )
        {

            await useCase.Execute(request, id);

            return NoContent();
        }

    }
}
