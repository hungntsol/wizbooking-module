using EventMessageBus.Events.Base;

namespace EventMessageBus.Abstracts;
public interface IMessageConsumer<in T> where T : IntegrationEventBase
{
	/// <summary>
	/// Consume a message in queue
	/// </summary>
	/// <param name="message"></param>
	/// <param name="cancellationToken"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	Task SubscribeAsync(T message, CancellationToken cancellationToken = default);
}
