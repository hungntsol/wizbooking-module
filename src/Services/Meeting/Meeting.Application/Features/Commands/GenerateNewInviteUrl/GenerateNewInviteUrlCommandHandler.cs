using MediatR;
using SharedCommon.Commons.HttpResponse;
using SharedCommon.Commons.LoggerAdapter;
using SharedCommon.Commons.Mediator.Command;
using SharedCommon.UnitOfWork;

namespace Meeting.Application.Features.Commands.GenerateNewInviteUrl;

public class
	GenerateNewInviteUrlCommandHandler : PlatformCommandHandler<GenerateNewInviteUrlCommand, JsonHttpResponse<Unit>>
{
	private readonly ILoggerAdapter<GenerateNewInviteUrlCommandHandler> _logger;

	public GenerateNewInviteUrlCommandHandler(
		IMediator mediator,
		IUnitOfWork unitOfWork,
		ILoggerAdapter<GenerateNewInviteUrlCommandHandler> logger) :
		base(mediator)
	{
		_logger = logger;
	}

	protected override Task<JsonHttpResponse<Unit>> InternalHandleAsync(
		GenerateNewInviteUrlCommand command,
		CancellationToken cancellationToken)
	{
		_logger.LogInformation("New invite url");
		return Task.FromResult(JsonHttpResponse<Unit>.Ok(Unit.Value));
	}
}
