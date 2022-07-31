using Identity.Application.Features.Commands.UpdateProfile;
using Identity.Application.Features.Queries.Profile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

public class ProfilesController : ApiV1ControllerBase
{
    public ProfilesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetProfiles(CancellationToken cancellation = default)
    {
        return JsonReponse(await mediator.Send(new GetProfileQuery(), cancellation));
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command, CancellationToken cancellation)
    {
        return JsonReponse(await mediator.Send(command, cancellation));
    }
}
