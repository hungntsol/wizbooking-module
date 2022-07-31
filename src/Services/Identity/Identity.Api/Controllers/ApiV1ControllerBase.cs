using Identity.Application.Commons;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiV1ControllerBase : ControllerBase
{
    protected readonly IMediator mediator;

    protected ApiV1ControllerBase(IMediator mediator)
    {
        this.mediator = mediator;
    }

    protected IActionResult ReturnJsonResponse<T>(JsonHttpResponse<T> response)
    {
        return StatusCode(response.Code, response);
    }
}
