using MediatR;

namespace SharedCommon.Commons.Mediator.Event;

public interface IPlatformInternalEventHandler<in TEvent> : INotificationHandler<TEvent>
	where TEvent : IPlatformInternalEvent
{
}
