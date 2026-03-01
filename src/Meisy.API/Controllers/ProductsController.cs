using Meisy.Application.UseCases.Inputs.Delete;
using Meisy.Application.UseCases.Products.Delete;
using Meisy.Application.UseCases.Products.Get;
using Meisy.Application.UseCases.Products.GetAll;
using Meisy.Application.UseCases.Products.Register;
using Meisy.Application.UseCases.Products.Update;
using Meisy.Communication.Requests.Products;
using Meisy.Communication.Responses;
using Meisy.Communication.Responses.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meisy.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(typeof(ResponseProductJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Register([FromBody] RequestRegisterProductJson request, [FromServices] IRegisterProductUseCase useCase)
        {
            var result = await useCase.Execute(request);
            return Created(string.Empty, result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<ResponseProductJson>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromServices] IGetAllProductUseCase useCase
            )
        {
            var result = await useCase.Execute();
            if (result.Count != 0) return Ok(result);

            return NoContent();
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDetailedProductJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(
            [FromServices] IGetProductUseCase useCase,
            [FromRoute] int id
            )
        {
            var result = await useCase.Execute(id);
            return Ok(result);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateProductUseCase useCase,
            [FromRoute] int id,
            [FromBody] RequestUpdateProductJson request
            )
        {
            await useCase.Execute(request,id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(
            [FromServices] IDeleteProductUseCase useCase,
            [FromRoute] int id
            )
        {
            await useCase.Execute(id);
            return NoContent();
        }



    }
}
