using System.Text;
using EventBusMessage.Abstracts;
using EventBusMessage.Events.Base;
using EventBusMessage.RabbitMQ.Settings;
using SharedCommon.Commons.LoggerAdapter;

namespace EventBusMessage.RabbitMQ;

internal class EventBusManager : IMessageProducer
{
	private const string MaxPriorityHeader = "x-max-priority";
	private readonly ILoggerAdapter<EventBusManager> _logger;
	private readonly EventBusQueueManager _queuesManager;
	private readonly RabbitMQManagerSettings _rabbitMqManagerSettings;

	private readonly IEventBusPersistence _rabbitMqPersistence;


	private IModel _channel = null!;

	public EventBusManager(IEventBusPersistence rabbitMqPersistence,
		RabbitMQManagerSettings rabbitMqManagerSettings,
		EventBusQueueManager queuesManager,
		ILoggerAdapter<EventBusManager> logger)
	{
		_rabbitMqPersistence = rabbitMqPersistence;
		_rabbitMqManagerSettings = rabbitMqManagerSettings;
		_queuesManager = queuesManager;
		_logger = logger;

		InitiateConnection();
	}

	public Task PublishAsync<T>(T message, string routingKey, int priority = 1) where T : IntegrationEventBase
	{
		var sendBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message,
			_rabbitMqManagerSettings.JsonSerializerOptions));

		return PublishMessageAsync(sendBytes.AsMemory(), routingKey, message.Id, priority);
	}

	public IModel GetChannel()
	{
		return _channel;
	}

	public void MarkAsComplete(BasicDeliverEventArgs args)
	{
		_channel.BasicAck(args.DeliveryTag, false);
	}

	public void MarkAsRejected(BasicDeliverEventArgs args)
	{
		_channel.BasicReject(args.DeliveryTag, false);
	}

	/// <summary>
	///     Publishes a message to a queue.
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

		_channel.BasicPublish(_rabbitMqManagerSettings.ExchangeName, routingKey, props, body);

		_logger.LogInformation("Published message `{MessageId}` with routing key `{Routing}`", messageId, routingKey);

		return Task.CompletedTask;
	}

	/// <summary>
	///     Init connection, channel
	/// </summary>
	private void InitiateConnection()
	{
		var conn = _rabbitMqPersistence.Connection;

		if (conn is null || !_rabbitMqPersistence.IsConnected)
		{
			_rabbitMqPersistence.TryConnect();
		}

		try
		{
			_channel = _rabbitMqPersistence.CreateChannel();
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
	///     Declare exchange for current channel
	/// </summary>
	private void ExchangeDeclare()
	{
		if (_rabbitMqManagerSettings.QueuePrefetch > 0)
		{
			_channel.BasicQos(0, _rabbitMqManagerSettings.QueuePrefetch, false);
		}

		_channel.ExchangeDeclare(
			_rabbitMqManagerSettings.ExchangeName,
			_rabbitMqManagerSettings.ExchangeType,
			true);
	}

	/// <summary>
	///     Declare queue and binding current channel.
	/// </summary>
	private void QueueDeclareAndBind()
	{
		foreach (var queue in _queuesManager.Queues)
		{
			var args = new Dictionary<string, object> { [MaxPriorityHeader] = 10 };

			_channel.QueueDeclare(queue.name, true, false, false, args);
			_channel.QueueBind(queue.name, _rabbitMqManagerSettings.ExchangeName, queue.binding, null);
		}
	}
}
