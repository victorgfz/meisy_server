using Meisy.Communication.Requests;
using Meisy.Communication.Responses;
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
        public async Task<IActionResult> Register(RequestRegisterProductJson request)
        {
            return NoContent();
        }


    }
}
