namespace EventMessageBus.Events.Base;
public abstract class IntegrationEventBase
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public IntegrationEventBase()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    protected IntegrationEventBase(Guid id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
    }
}
