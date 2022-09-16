using System.Net.Sockets;
using MongoDB.Driver;
using Persistence.MongoDb.Abstract;
using Persistence.MongoDb.Attribute;
using Persistence.MongoDb.Internal;
using Polly;
using Polly.Retry;
using SharedCommon.Modules.LoggerAdapter;

namespace Persistence.MongoDb.Data;

public abstract class MongoDbContext : IMongoDbContext
{
	private readonly List<Func<Task>> _commands;
	private readonly MongoContextConfiguration _contextConfiguration;
	private readonly object _lock = new();
	private readonly ILoggerAdapter<MongoDbContext> _logger;
	private readonly RetryPolicy _retryPolicy;

	protected MongoDbContext(MongoContextConfiguration contextConfiguration,
		ILoggerAdapter<MongoDbContext> logger)
	{
		_contextConfiguration = contextConfiguration;
		_logger = logger;
		_commands = new List<Func<Task>>();

		_retryPolicy = Policy.Handle<SocketException>()
			.Or<Exception>()
			.WaitAndRetry(5, count => TimeSpan.FromSeconds(Math.Pow(2, count)), (ex, time) =>
			{
				_logger.LogWarning(ex, "MongoDbContext cannot be reached after {TimeOut}s ({ExceptionMessage})",
					$"{time.TotalSeconds:n1}", ex.Message);
			});
	}

	private IMongoDatabase? MongoDatabase { get; set; }
	public IClientSessionHandle? SessionHandle { get; set; }
	public MongoClient? Client { get; set; }

	public void AddCommand(Func<Task> func)
	{
		_commands.Add(func);
	}

	public IClientSessionHandle GetSessionHandle()
	{
		OnConnect();

		return SessionHandle ?? Client!.StartSession();
	}

	public async Task<int> SaveChanges()
	{
		OnConnect();

		using (SessionHandle = await Client!.StartSessionAsync())
		{
			SessionHandle.StartTransaction();

			var commandTasks = _commands.Select(command => command());
			await Task.WhenAll(commandTasks);

			try
			{
				await SessionHandle.CommitTransactionAsync();
			}
			catch (Exception e)
			{
				_logger.LogCritical(e, "{Message}", e.Message);
				await SessionHandle.AbortTransactionAsync();
			}
		}

		return _commands.Count;
	}

	public IMongoCollection<TDocument> GetCollection<TDocument>() where TDocument : class, IDocumentEntity
	{
		OnConnect();

		return MongoDatabase!.GetCollection<TDocument>(Bson.CollectionName<TDocument>());
	}

	public IMongoDatabase GetDatabase()
	{
		OnConnect();

		return MongoDatabase!;
	}

	public virtual Task InternalCreateIndexesAsync(bool recreate = false)
	{
		return Task.CompletedTask;
	}

	/// <summary>
	/// Turn on connection
	/// </summary>
	private void OnConnect()
	{
		if (Client is not null)
		{
			return;
		}

		Connect();
	}

	/// <summary>
	/// Execute connect to MongoDB
	/// </summary>
	private void Connect()
	{
		lock (_lock)
		{
			_retryPolicy.Execute(ConfigureDbContext);
		}
	}

	/// <summary>
	/// Configure Client and MongoDatabase from setting
	/// </summary>
	private void ConfigureDbContext()
	{
		Client = _contextConfiguration.ClientSettings is not null
			? new MongoClient(_contextConfiguration.ClientSettings)
			: new MongoClient(_contextConfiguration.Connection);
		MongoDatabase = Client.GetDatabase(_contextConfiguration.DatabaseName);
	}
}
