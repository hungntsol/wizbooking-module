using SharedCommon.Commons.Entity;
using SharedCommon.Modules.LoggerAdapter;
using SharedCommon.Modules.Mediator.Event;

namespace Identity.Application.InternalNotifications;

public class NewAccountInternalInternalEventHandler : InternalEventHandler<UserAccount>
{
	private readonly ILoggerAdapter<NewAccountInternalInternalEventHandler> _logger;


	public NewAccountInternalInternalEventHandler(
		ILoggerAdapter<NewAccountInternalInternalEventHandler> logger)
	{
		_logger = logger;
	}

	protected override Task HandleAsync(DomainEvent<UserAccount> @event,
		CancellationToken cancellationToken = default)
	{
		if (@event.EventAction.Equals(DomainEventAction.Created))
		{
			_logger.LogInformation("Register new account {Email}", @event.EventData!.Email);
		}

		return Task.CompletedTask;
	}
}
