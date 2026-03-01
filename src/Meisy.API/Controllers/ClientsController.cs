using Meisy.Application.UseCases.Clients.Delete;
using Meisy.Application.UseCases.Clients.GetAll;
using Meisy.Application.UseCases.Clients.Register;
using Meisy.Application.UseCases.Clients.Update;
using Meisy.Application.UseCases.Inputs.Register;
using Meisy.Communication.Requests.Clients;
using Meisy.Communication.Requests.Inputs;
using Meisy.Communication.Responses;
using Meisy.Communication.Responses.Clients;
using Meisy.Communication.Responses.Inputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meisy.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(typeof(ResponseClientJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
          [FromServices] IRegisterClientUseCase useCase,
          [FromBody] RequestClientJson request
          )
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);

        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ResponseClientJson>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllClientUseCase useCase
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
            [FromServices] IUpdateClientUseCase useCase,
            [FromBody] RequestClientJson request,
            [FromRoute] int id
            )
        {
            await useCase.Execute(request, id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteClientUseCase useCase,
            [FromRoute] int id
            )
        {
            await useCase.Execute(id);
            return NoContent();
        }
    }
}
