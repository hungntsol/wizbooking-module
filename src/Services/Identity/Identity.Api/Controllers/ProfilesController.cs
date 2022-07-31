using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

public class ProfilesController : ApiV1ControllerBase
{
    public ProfilesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> GetProfiles()
    {
        return Ok();
    }
}
