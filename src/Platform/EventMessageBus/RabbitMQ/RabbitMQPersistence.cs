using EventMessageBus.RabbitMQ.Settings;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace EventMessageBus.RabbitMQ;
public class RabbitMQPersistence : IRabbitMQPersistence
{
	private readonly IConnectionFactory _connectionFactory;
	private readonly ILogger<RabbitMQPersistence> _logger;
	private readonly int _retryCount;

	private IConnection? _connection;
	private bool _disposed;

	private readonly object _syncRoot = new();

	public RabbitMQPersistence(
		ILogger<RabbitMQPersistence> logger,
		RabbitMQManagerSettings managerSettings)
	{
		_logger = logger;
		_connectionFactory = new ConnectionFactory() { Uri = new Uri(managerSettings.HostAddress) };
		_retryCount = managerSettings.RetryCount;

		_connection = _connectionFactory.CreateConnection();
	}

	public void Dispose()
	{
		if (!_disposed || this.IsConnected)
			return;

		_disposed = true;

		try
		{
			_connection!.ConnectionBlocked -= OnConnectionBlocked;
			_connection.ConnectionShutdown -= OnConnectionShutdown;
			_connection.CallbackException -= OnCallbackException;
			_connection.Dispose();
		}
		catch (Exception e)
		{
			_logger.LogCritical(e, "{Message}", e.Message);
			throw;
		}
	}

	public bool IsConnected => _connection is not null && _connection.IsOpen && !_disposed;

	public bool TryConnect()
	{
		_logger.LogWarning("Attempt to re-connect to RabbitMQ");

		lock (_syncRoot)
		{
			var policy = Policy.Handle<SocketException>()
				.Or<BrokerUnreachableException>()
				.WaitAndRetry(_retryCount, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), (ex, time) =>
				{
					_logger.LogWarning(ex, "RabbitMQ cannot be reached after {TimeOut}s ({ExceptionMessage})",
						$"{time.TotalSeconds:n1}", ex.Message);
				});

			policy.Execute(() => { _connection = _connectionFactory.CreateConnection(); });

			if (this.IsConnected)
			{
				_connection!.ConnectionShutdown += OnConnectionShutdown;
				_connection.ConnectionBlocked += OnConnectionBlocked;
				_connection.CallbackException += OnCallbackException;

				_logger.LogInformation(
					"RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events",
					_connection.Endpoint.HostName);

				return true;
			}

			_logger.LogCritical("RabbitMQ cannot be created or opened");
			return false;
		}
	}

	public IConnection? Connection => _connection;

	public IModel CreateChannel()
	{
		if (!IsConnected)
		{
			_logger.LogCritical("No RabbitMQ is available");
			throw new Exception();
		}

		return _connection!.CreateModel();
	}

	private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
	{
		if (_disposed)
			return;
		_logger.LogWarning("RabbitMQ connection is blocked. Trying to connect again");

		this.TryConnect();
	}

	private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
	{
		if (_disposed)
			return;
		_logger.LogWarning("RabbitMQ connection is shutdown. Trying to connect again");

		this.TryConnect();
	}

	private void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
	{
		if (_disposed)
			return;

		_logger.LogWarning("RabbitMQ connection is broken. Trying to connect again");
		this.TryConnect();
	}
}
