using SharedCommon.Commons.Notification;
using SharedCommon.Domain;

namespace Identity.Application.InternalNotifications;

public class RegisterNewAccountNotificationEventHandler : NotificationEventHandler<UserAccount>
{
    private readonly ILoggerAdapter<RegisterNewAccountNotificationEventHandler> _loggerAdapter;


    public RegisterNewAccountNotificationEventHandler(
        ILoggerAdapter<RegisterNewAccountNotificationEventHandler> loggerAdapter)
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
