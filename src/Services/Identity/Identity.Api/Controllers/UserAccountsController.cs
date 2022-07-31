using Identity.Application.Features.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

public class UserAccountsController : ApiV1ControllerBase
{
    public UserAccountsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterNewAccountCommand command)
    {
        return ReturnJsonResponse(await mediator.Send(command));
    }
}
