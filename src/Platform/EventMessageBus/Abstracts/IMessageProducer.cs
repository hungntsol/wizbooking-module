using EventMessageBus.Events.Base;

namespace EventMessageBus.Abstracts;
public interface IMessageProducer
{
	/// <summary>
	/// Publish message from producer to consumer
	/// </summary>
	/// <param name="message"></param>
	/// <param name="routingKey"></param>
	/// <param name="priority"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	Task PublishAsync<T>(T message, string routingKey, int priority = 1) where T : IntegrationEventBase;

	/// <summary>
	/// Get channel which is used to publish message
	/// </summary>
	/// <returns></returns>
	IModel GetChannel();

	/// <summary>
	/// Mark a message is successfully processed
	/// </summary>
	/// <param name="args"></param>
	void MarkAsComplete(BasicDeliverEventArgs args);

	/// <summary>
	/// Mark a message is failed to produce.
	/// </summary>
	/// <param name="args"></param>
	void MarkAsRejected(BasicDeliverEventArgs args);
}
