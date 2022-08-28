using SharedCommon.Commons.Mediator.Event;

namespace SharedCommon.Commons.Mediator.Command;

public abstract class PlatformCommandEvent : IPlatformInternalEvent
{
	public abstract string EventName { get; }
	public Guid Id { get; init; }
	public DateTime CreatedAt { get; } = DateTime.UtcNow;
	public string? CreatedBy { get; set; }
	public abstract string EventAction { get; }

	public static string EventNameValue<TCommand>()
	{
		return typeof(TCommand).Name;
	}
}

public class PlatformCommandEvent<TCommand> : PlatformCommandEvent where TCommand : IPlatformCommand
{
	public PlatformCommandEvent()
	{
	}

	public PlatformCommandEvent(TCommand command, PlatformInternalEventAction action)
	{
		Id = command.GetAuditedTrackedId();
		Data = command;
		Action = action;
	}

	public TCommand Data { get; set; } = default!;
	public PlatformInternalEventAction Action { get; set; }

	public override string EventName => EventNameValue<TCommand>();
	public override string EventAction => Action.ToString();
}

public enum PlatformInternalEventAction
{
	Executing,
	Executed
}
