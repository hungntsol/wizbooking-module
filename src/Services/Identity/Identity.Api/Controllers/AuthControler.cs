using Identity.Application.Features.Login;
using Identity.Application.Features.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

public class AuthControler : ApiV1ControllerBase
{
    public AuthControler(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterNewAccountCommand command)
    {
        return ReturnJsonResponse(await mediator.Send(command));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        return ReturnJsonResponse(await mediator.Send(command));
    }
}
