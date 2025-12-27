using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meisy.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [Authorize]
    public class InputsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register()
        {

            return Ok();
        }

    }
}
