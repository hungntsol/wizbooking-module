using EventBusMessage.Options;

namespace EventBusMessage.RabbitMQ.Settings;
public class RabbitMQManagerSettings
{
    public string HostAddress { get; set; } = null!;
    public string ExchangeName { get; set; } = null!;
    public string ExchangeType { get; set; } = global::RabbitMQ.Client.ExchangeType.Direct;
    public ushort QueuePrefetch { get; set; }
    public int RetryCount { get; set; }
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = JsonOptions.Default;
}
