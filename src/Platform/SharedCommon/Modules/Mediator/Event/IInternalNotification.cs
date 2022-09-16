using MediatR;

namespace SharedCommon.Modules.Mediator.Event;

public interface IInternalNotification : INotification
{
	public Guid Id { get; init; }
	public DateTime CreatedAt { get; }
	public string? CreatedBy { get; set; }
	public string EventName { get; }
	public string EventAction { get; }
}
