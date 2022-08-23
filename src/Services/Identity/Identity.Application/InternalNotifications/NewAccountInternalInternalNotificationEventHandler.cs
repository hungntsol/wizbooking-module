using SharedCommon.Commons.NotificationEvent;
using SharedCommon.Domain;

namespace Identity.Application.InternalNotifications;

public class NewAccountInternalInternalNotificationEventHandler : InternalNotificationEventHandler<UserAccount>
{
	private readonly ILoggerAdapter<NewAccountInternalInternalNotificationEventHandler> _loggerAdapter;


	public NewAccountInternalInternalNotificationEventHandler(
		ILoggerAdapter<NewAccountInternalInternalNotificationEventHandler> loggerAdapter)
	{
		_loggerAdapter = loggerAdapter;
	}

	protected override Task HandleAsync(DomainEvent<UserAccount> @event, CancellationToken cancellationToken = default)
	{
		if (@event.EventAction.Equals(DomainEventAction.Created))
		{
			_loggerAdapter.LogInformation("Register new account {Email}", @event.EventData!.Email);
		}

		return Task.CompletedTask;
	}
}
