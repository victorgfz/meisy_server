using Meisy.Application.UseCases.Users.GetProfile;
using Meisy.Communication.Responses;
using Meisy.Communication.Responses.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meisy.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        [HttpGet("profile")]
        [ProducesResponseType(typeof(ResponseProfileJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile([FromServices] IGetProfileUseCase useCase)
        {
            var result = await useCase.Execute();

            return Ok(result);
        }
    }
}
