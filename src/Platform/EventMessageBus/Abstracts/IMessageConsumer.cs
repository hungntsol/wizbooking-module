using EventBusMessage.Events.Base;

namespace EventBusMessage.Abstracts;

public interface IMessageConsumer
{

}

public interface IMessageConsumer<in T> : IMessageConsumer where T : IntegrationEventBase
{
    /// <summary>
    /// Consume a message in queue
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task Consume(T message, CancellationToken cancellationToken = default);
}
