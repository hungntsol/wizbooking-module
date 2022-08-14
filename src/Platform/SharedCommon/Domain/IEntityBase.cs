namespace SharedCommon.Domain;

/// <summary>
///     Base entity properties
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IEntityBase<TKey>
{
    TKey Id { get; set; }

    DateTime CreatedAt { get; set; }

    DateTime ModifiedAt { get; set; }
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
