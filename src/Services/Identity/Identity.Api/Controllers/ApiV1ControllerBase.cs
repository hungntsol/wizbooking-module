using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedCommon.Commons.HttpResponse;

namespace Identity.Api.Controllers;

[ApiController]
[Route("v1/api/[controller]")]
public abstract class ApiV1ControllerBase : ControllerBase
{
	protected readonly IMediator Mediator;

	protected ApiV1ControllerBase(IMediator mediator)
	{
		Mediator = mediator;
	}

	protected IActionResult JsonResponse(JsonHttpResponse response)
	{
		return StatusCode(response.Status, response);
	}
}
