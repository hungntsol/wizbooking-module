namespace SharedCommon.Commons.Entity;

/// <summary>
/// Contract entity must have CreatedAt field
/// </summary>
public interface ICreatedAt
{
	DateTime CreatedAt { get; set; }
}

/// <summary>
/// Contract entity must have ModifiedAt field
/// </summary>
public interface IModifiedAt
{
	DateTime ModifiedAt { get; set; }
}

public interface IDatetimeEntityBase : ICreatedAt, IModifiedAt
{
}

/// <summary>
///     Base entity properties
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IEntityBase<TKey> : IDatetimeEntityBase
{
	TKey Id { get; set; }
}

/// <summary>
///     Support payload event for entity base.
/// </summary>
public interface ISupportPayloadEvent
{
	List<KeyValuePair<string, object?>> PayloadEvents { get; }

	/// <summary>
	///     Add payload event content
	/// </summary>
	/// <param name="payloadEventName"></param>
	/// <param name="eventPayload"></param>
	void AppendPayloadEvent(string payloadEventName, object? eventPayload = null);

	KeyValuePair<string, object?> FindPayload(string keyName);
	TPayload? FindPayload<TPayload>(string keyName);

	bool Has(string keyName);
}
