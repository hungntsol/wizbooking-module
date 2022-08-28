using MediatR;

namespace SharedCommon.Commons.Mediator.Event;

public interface IPlatformInternalEvent : INotification
{
	public Guid Id { get; init; }
	public DateTime CreatedAt { get; }
	public string? CreatedBy { get; set; }
	public string EventType { get; }
	public string EventName { get; }
	public string EventAction { get; }
}
