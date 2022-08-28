using MediatR;
using Meeting.Application.Features.HostUserSupply.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Meeting.Api.Controllers;

public class HostUserController : ApiV1ControllerBase
{
	public HostUserController(IMediator mediator) : base(mediator)
	{
	}

	[HttpPost]
	[Authorize]
	public async Task<IActionResult> NewSupply(CreateNewHostUserSupplyingCommand commandRequest)

	{
		return JsonResponse(await Mediator.Send(commandRequest));
	}
}
