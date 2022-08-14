using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace SharedCommon.Domain;

public abstract class EntityBase<TKey> : IEntityBase<TKey>, ISupportPayloadEvent
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public TKey Id { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
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
