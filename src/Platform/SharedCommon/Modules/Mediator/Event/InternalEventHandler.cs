using MediatR;
using SharedCommon.Commons.Entity;

namespace SharedCommon.Modules.Mediator.Event;

public abstract class InternalEventHandler<TEvent> : INotificationHandler<DomainEvent<TEvent>>
	where TEvent : class
{
	public virtual async Task Handle(DomainEvent<TEvent> notification, CancellationToken cancellationToken)
	{
		await HandleAsync(notification, cancellationToken);
	}

	protected abstract Task HandleAsync(DomainEvent<TEvent> @event,
		CancellationToken cancellationToken = default);
}
