using System.Net.Sockets;
using MongoDB.Driver;
using Persistence.MongoDb.Abstract;
using Persistence.MongoDb.Attribute;
using Persistence.MongoDb.Internal;
using Polly;
using Polly.Retry;
using SharedCommon.Commons.LoggerAdapter;

namespace Persistence.MongoDb.Data;

public abstract class MongoDbContext : IMongoDbContext
{
	private readonly List<Func<Task>> _commands;
	private readonly MongoContextConfiguration _configuration;
	private readonly object _lock = new();
	private readonly ILoggerAdapter<MongoDbContext> _loggerAdapter;
	private readonly RetryPolicy _retryPolicy;

	public MongoDbContext(MongoContextConfiguration configuration, ILoggerAdapter<MongoDbContext> loggerAdapter)
	{
		_configuration = configuration;
		_loggerAdapter = loggerAdapter;
		_commands = new List<Func<Task>>();

		_retryPolicy = Policy.Handle<SocketException>()
			.Or<Exception>()
			.WaitAndRetry(5, count => TimeSpan.FromSeconds(Math.Pow(2, count)), (ex, time) =>
			{
				_loggerAdapter.LogWarning(ex, "MongoDbContext cannot be reached after {TimeOut}s ({ExceptionMessage})",
					$"{time.TotalSeconds:n1}", ex.Message);
			});
	}

	private IMongoDatabase? MongoDatabase { get; set; }
	public IClientSessionHandle? SessionHandle { get; set; }
	public MongoClient? Client { get; set; }


	public void Dispose()
	{
		SessionHandle?.Dispose();
		GC.SuppressFinalize(this);
	}

	public void AddCommand(Func<Task> func)
	{
		_commands.Add(func);
	}

	public async Task<int> SaveChanges()
	{
		Configure();

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
				_loggerAdapter.LogCritical(e, "{Message}", e.Message);
				await SessionHandle.AbortTransactionAsync();
			}
		}

		return _commands.Count;
	}

	public IMongoCollection<TDocument> GetCollection<TDocument>() where TDocument : class, IDocument
	{
		Configure();

		return MongoDatabase!.GetCollection<TDocument>(GetCollectionName<TDocument>());
	}

	public IMongoDatabase GetDatabase()
	{
		Configure();

		return MongoDatabase!;
	}

	public virtual Task InternalCreateIndexesAsync(bool recreate = false)
	{
		return Task.CompletedTask;
	}

	private void Configure()
	{
		if (Client is not null)
		{
			return;
		}

		Connect();
	}

	private void Connect()
	{
		lock (_lock)
		{
			_retryPolicy.Execute(() =>
			{
				Client = new MongoClient(_configuration.Connection);
				MongoDatabase = Client.GetDatabase(_configuration.DatabaseName);
			});
		}
	}

	private static string GetCollectionName<TDocument>() where TDocument : class, IDocument
	{
		return Bson.CollectionName<TDocument>();
	}
}
