using MediatR;

namespace SharedCommon.Modules.Mediator.Event;

public interface IInternalNotificationHandler<in TEvent> : INotificationHandler<TEvent>
	where TEvent : IInternalNotification
{
}
