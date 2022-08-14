using MediatR;

namespace SharedCommon.Domain;

public class DomainEvent<TEntity> : INotification where TEntity : class
{
    public DomainEvent()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public DomainEvent(string name, DomainEventAction action, TEntity? eventData = null) : this()
    {
        EventName = name;
        EventAction = action;
        EventData = eventData;
    }

    public DomainEvent(DomainEventAction action, TEntity? data = null) :
        this(nameof(TEntity), action, data)
    {
    }

    public Guid Id { get; set; }
    public string EventName { get; set; } = null!;
    public DomainEventAction EventAction { get; set; }
    public TEntity? EventData { get; set; }
    public DateTime CreatedAt { get; set; }

    public static DomainEvent<TEntity> New(DomainEventAction action, TEntity? data = null)
    {
        return new DomainEvent<TEntity>(action, data);
    }

    public static DomainEvent<TEntity> New(string eventName, DomainEventAction action, TEntity? data = null)
    {
        return new DomainEvent<TEntity>(eventName, action, data);
    }
}

public enum DomainEventAction
{
    Queried,
    Created,
    Updated,
    Deleted
}
