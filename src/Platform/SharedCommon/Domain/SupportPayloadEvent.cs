using System.Text.Json;

namespace SharedCommon.Domain;

public class SupportPayloadEvent
{
	public List<KeyValuePair<string, object?>> PayloadEvents { get; } = new();

	public void AppendPayloadEvent(string payloadEventName, object? eventPayload = null)
	{
		PayloadEvents.Add(new KeyValuePair<string, object?>(payloadEventName, eventPayload));
	}

	public virtual KeyValuePair<string, object?> FindPayload(string keyName)
	{
		return PayloadEvents.Count == 0 ? default : PayloadEvents.FirstOrDefault(q => q.Key.Equals(keyName));
	}

	public virtual TPayload? FindPayload<TPayload>(string keyName)
	{
		var payload = FindPayload(keyName);
		return JsonSerializer.Deserialize<TPayload>(payload.Value?.ToString()!);
	}

	public bool Has(string keyName)
	{
		return PayloadEvents.Count != 0 && PayloadEvents.Select(q => q.Key).ToHashSet().Contains(keyName);
	}
}
