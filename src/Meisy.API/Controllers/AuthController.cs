using Meisy.Application.UseCases.Auth.Login;
using Meisy.Application.UseCases.Auth.Register;
using Meisy.Communication.Requests;
using Meisy.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Meisy.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        [HttpPost("register")]
        [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterUserUseCase useCase,
            [FromBody] RequestRegisterUserJson request
            )
        {
            
            var response = await useCase.Execute(request);


            return Created(string.Empty, response);
        }


        [HttpPost("login")]
        [ProducesResponseType(typeof(ResponseLoginJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromServices] ILoginUseCase useCase,
            [FromBody] RequestLoginJson request
            )
        {
            var result = await useCase.Execute(request);

            return Ok(result);
        }

    }
}
