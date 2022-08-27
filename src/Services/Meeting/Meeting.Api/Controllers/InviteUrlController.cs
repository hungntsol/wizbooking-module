using MediatR;
using Meeting.Application.Features.Commands.GenerateNewInviteUrl;
using Microsoft.AspNetCore.Mvc;
using Persistence.MongoDb.Attribute;

namespace Meeting.Api.Controllers;

public class InviteUrlController : ApiV1ControllerBase
{
	public InviteUrlController(IMediator mediator) : base(mediator)
	{
	}


	[HttpPost("new")]
	[MongoTransactional]
	public async Task<IActionResult> NewInviteUrl(GenerateNewInviteUrlCommand commandRequest)
	{
		var result = await Mediator.Send(commandRequest);
		return JsonResponse(result);
	}
}
