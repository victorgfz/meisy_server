using Meisy.Application.UseCases.Notifications.GetPreferences;
using Meisy.Application.UseCases.Notifications.Register;
using Meisy.Application.UseCases.Notifications.UpdatePreferences;
using Meisy.Communication.Requests.Notification;
using Meisy.Communication.Responses;
using Meisy.Communication.Responses.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meisy.API.Controllers;

[Route("/[controller]")]
[ApiController]
[Authorize]
public class NotificationController : ControllerBase
{
    [HttpGet("vapid-public-key")]
    [ProducesResponseType(typeof(ResponseVapidPublicKeyJson), StatusCodes.Status200OK)]
    public IActionResult GetVapidPublicKey([FromServices] IConfiguration configuration)
    {
        var publicKey = configuration["Settings:WebPush:PublicKey"] ?? string.Empty;

        return Ok(new ResponseVapidPublicKeyJson
        {
            PublicKey = publicKey,
        });
    }

    [HttpPost("subscribe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Subscribe(
        [FromBody] RequestPushSubscriptionJson request,
        [FromServices] IRegisterPushSubscriptionUseCase useCase)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    [HttpGet("preferences")]
    [ProducesResponseType(typeof(ResponseNotificationPreferencesJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPreferences(
        [FromQuery] string endpoint,
        [FromServices] IGetNotificationPreferencesUseCase useCase)
    {
        var response = await useCase.Execute(endpoint);

        return Ok(response);
    }

    [HttpPut("preferences")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePreferences(
        [FromBody] RequestUpdatePreferencesJson request,
        [FromServices] IUpdateNotificationPreferencesUseCase useCase)
    {
        await useCase.Execute(request);

        return NoContent();
    }
}
