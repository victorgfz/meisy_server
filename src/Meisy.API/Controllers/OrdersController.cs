using Meisy.Application.UseCases.Orders.GetAll;
using Meisy.Application.UseCases.Orders.Register;
using Meisy.Application.UseCases.Orders.Update;
using Meisy.Application.UseCases.Products.GetAll;
using Meisy.Application.UseCases.Products.Register;
using Meisy.Communication.Requests.Orders;
using Meisy.Communication.Requests.Products;
using Meisy.Communication.Responses;
using Meisy.Communication.Responses.Orders;
using Meisy.Communication.Responses.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meisy.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [Authorize]

    public class OrdersController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseOrderJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RequestRegisterOrderJson request, [FromServices] IRegisterOrderUseCase useCase)
        {
            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ResponseDetailedOrderJson>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllOrderUseCase useCase
            )
        {
            var result = await useCase.Execute();
            if (result.Count != 0) return Ok(result);
            return NoContent();
        }

        [HttpPatch("{id}/update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateStatus(
            [FromRoute] int id,
            [FromServices] IUpdateOrderStatusUseCase useCase
            )
        {
            await useCase.Execute(id);
            return NoContent();
        }


        [HttpPatch("{id}/cancel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CancelStatus(
            [FromRoute] int id,
            [FromServices] ICancelOrderStatusUseCase useCase
            )
        {
            await useCase.Execute(id);
            return NoContent();
        }

    }
}
