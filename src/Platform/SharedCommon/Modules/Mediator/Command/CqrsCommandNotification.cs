using SharedCommon.Modules.Mediator.Event;

namespace SharedCommon.Modules.Mediator.Command;

public class CqrsCommandNotification<TCommand> : CqrsCommandNotification where TCommand : ICqrsCommand
{
	public CqrsCommandNotification()
	{
	}

	public CqrsCommandNotification(TCommand command, PlatformInternalEventAction action)
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

public abstract class CqrsCommandNotification : IInternalNotification
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

public enum PlatformInternalEventAction
{
	Executing,
	Executed
}
