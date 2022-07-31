using EventMessageBus.Abstracts;
using EventMessageBus.Events.Base;
using EventMessageBus.RabbitMQ.Settings;
using Microsoft.Extensions.Logging;
using System.Text;

namespace EventMessageBus.RabbitMQ;
internal class RabbitMQMessageManager : IMessageProducer
{
	private const string MaxPriorityHeader = "x-max-priority";


	private IModel _channel = null!;

	private readonly IRabbitMQPersistence _rabbitMQPersistence;
	private readonly ILogger<RabbitMQMessageManager> _logger;
	private readonly RabbitMQManagerSettings _rabbitMQManagerSettings;
	private readonly RabbitMQQueueManager _queuesManager;

	public RabbitMQMessageManager(IRabbitMQPersistence rabbitMQPersistence,
		RabbitMQManagerSettings rabbitMQManagerSettings,
		RabbitMQQueueManager queuesManager,
		ILogger<RabbitMQMessageManager> logger)
	{
		_rabbitMQPersistence = rabbitMQPersistence;
		_rabbitMQManagerSettings = rabbitMQManagerSettings;
		_queuesManager = queuesManager;
		_logger = logger;

		InitiateConnection();
	}

	public Task PublishAsync<T>(T message, string routingKey, int priority = 1) where T : IntegrationEventBase
	{
		var sendBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message,
			_rabbitMQManagerSettings.JsonSerializerOptions));

		return PublishMessageAsync(sendBytes.AsMemory(), routingKey, message.Id, priority);
	}

	public IModel GetChannel()
	{
		return _channel;
	}

	public void MarkAsComplete(BasicDeliverEventArgs args) => _channel.BasicAck(args.DeliveryTag, false);

	public void MarkAsRejected(BasicDeliverEventArgs args) => _channel.BasicReject(args.DeliveryTag, false);

	/// <summary>
	/// Publishes a message to a queue.
	/// </summary>
	/// <param name="body"></param>
	/// <param name="routingKey"></param>
	/// <param name="messageId"></param>
	/// <param name="priority"></param>
	/// <returns></returns>
	private Task PublishMessageAsync(ReadOnlyMemory<byte> body, string routingKey, Guid messageId, int priority = 1)
	{
		var props = _channel.CreateBasicProperties();
		props.Persistent = false;
		props.Priority = Convert.ToByte(priority);

		_channel.BasicPublish(_rabbitMQManagerSettings.ExchangeName, routingKey, props, body);

		_logger.LogInformation("Published message `{MessageId}` with routing key `{Routing}`", messageId, routingKey);

		return Task.CompletedTask;
	}

	/// <summary>
	/// Init connection, channel
	/// </summary>
	private void InitiateConnection()
	{
		var conn = _rabbitMQPersistence.Connection;

		if (conn is null || !_rabbitMQPersistence.IsConnected)
		{
			_rabbitMQPersistence.TryConnect();
		}

		try
		{
			_channel = _rabbitMQPersistence.CreateChannel();
		}
		catch (Exception e)
		{
			_logger.LogCritical(e, "FATAL ERROR: RabbitMQ connection could not be established");
			throw;
		}

		ExchangeDeclare();
		QueueDeclareAndBind();
	}

	/// <summary>
	/// Declare exchange for current channel
	/// </summary>
	private void ExchangeDeclare()
	{
		if (_rabbitMQManagerSettings.QueuePrefetch > 0)
		{
			_channel.BasicQos(0, _rabbitMQManagerSettings.QueuePrefetch, false);
		}

		_channel.ExchangeDeclare(
			_rabbitMQManagerSettings.ExchangeName,
			_rabbitMQManagerSettings.ExchangeType,
			true);
	}

	/// <summary>
	/// Declare queue and binding current channel.
	/// </summary>
	private void QueueDeclareAndBind()
	{
		foreach (var queue in _queuesManager.Queues)
		{
			var args = new Dictionary<string, object>() { [MaxPriorityHeader] = 10 };

			_channel.QueueDeclare(queue.name, true, false, false, args);
			_channel.QueueBind(queue.name, _rabbitMQManagerSettings.ExchangeName, queue.binding, null);
		}
	}
}
