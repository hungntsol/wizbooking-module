namespace EventBusMessage.Events.Base;

public abstract class IntegrationEventBase
{
	protected IntegrationEventBase()
	{
		Id = Guid.NewGuid();
		CreatedAt = DateTime.UtcNow;
	}

	public Guid Id { get; init; }
	public DateTime CreatedAt { get; init; }
}
