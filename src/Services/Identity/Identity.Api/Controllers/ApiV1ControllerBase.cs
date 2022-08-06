using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedCommon.Commons.HttpResponse;

namespace Identity.Api.Controllers;

[ApiController]
[Route("v1/api/[controller]")]
public abstract class ApiV1ControllerBase : ControllerBase
{
    protected readonly IMediator mediator;

    protected ApiV1ControllerBase(IMediator mediator)
    {
        this.mediator = mediator;
    }

    protected IActionResult JsonReponse<T>(JsonHttpResponse<T> response)
    {
        return StatusCode(response.Status, response);
    }
}