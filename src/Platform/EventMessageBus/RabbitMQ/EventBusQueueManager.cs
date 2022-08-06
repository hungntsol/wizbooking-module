namespace EventBusMessage.RabbitMQ;
public class EventBusQueueManager
{
    internal readonly IList<(string name, Type type, string binding)> Queues = new List<(string name, Type type, string binding)>();

    public void Add<T>(string? queueName = null, string? bindingKey = null) where T : class
    {
        var type = typeof(T);
        var binding = bindingKey ?? queueName ?? type.FullName;

        Queues.Add((queueName ?? type.FullName, type, binding)!);
    }
}
