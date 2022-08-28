using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistence.MongoDb.Attribute;

namespace Meeting.Api.Controllers;

public class HostUserController : ApiV1ControllerBase
{
	public HostUserController(IMediator mediator) : base(mediator)
	{
	}

	[HttpPost]
	[MongoTransactional]
	public async Task<IActionResult> NewSupply()
	{
		return Ok();
	}
}
