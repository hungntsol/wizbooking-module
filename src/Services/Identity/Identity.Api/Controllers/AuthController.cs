using Identity.Application.Features.Commands.Login;
using Identity.Application.Features.Commands.Register;
using Identity.Application.Features.Queries.ConfirmAccount;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

public class AuthController : ApiV1ControllerBase
{
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterNewAccountCommand command)
    {
        return JsonReponse(await mediator.Send(command));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        return JsonReponse(await mediator.Send(command));
    }

    [HttpGet("/auth/confirm/me")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmAccount([FromQuery] ConfirmAccountQuery query)
    {
        return JsonReponse(await mediator.Send(query));
    }
}