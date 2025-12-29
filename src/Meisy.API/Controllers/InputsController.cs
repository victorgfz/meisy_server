using Meisy.Application.UseCases.Inputs.Register;
using Meisy.Communication.Requests;
using Meisy.Communication.Responses;
using Meisy.Communication.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Meisy.Application.UseCases.Inputs.GetAll;
using Meisy.Application.UseCases.Inputs.Update;
using Meisy.Application.UseCases.Inputs.Delete;

namespace Meisy.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [Authorize]
    public class InputsController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseInputJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterInputUseCase useCase,
            [FromBody] RequestInputJson request
            )
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);

        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ResponseInputJson>), StatusCodes.Status200OK)]
        [ProducesResponseType( StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
            [FromQuery]InputType type,
            [FromServices] IGetAllInputUseCase useCase)
        {
            var response = await useCase.Execute(type);
            if (response.Count != 0) return Ok(response);
            return NoContent();
        }

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromBody] RequestInputJson request,
            [FromServices] IUpdateInputUseCase useCase
            )
        {
            await useCase.Execute(id, request);

            return NoContent();
        }


        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete
            ([FromRoute] int id,
            [FromServices] IDeleteInputUseCase useCase

            )
        {
            await useCase.Execute(id);

            return NoContent();
        }
    }
}
