using MongoDB.Driver;
using Persistence.MongoDb.Abstract;
using SharedCommon.UnitOfWork;

namespace Persistence.MongoDb.Data;

public class UnitOfWork : IUnitOfWork
{
	private readonly IMongoDbContext _dbContext;
	private readonly IClientSessionHandle _sessionHandle;


	public UnitOfWork(IMongoDbContext dbContext)
	{
		_dbContext = dbContext;
		_sessionHandle = _dbContext.GetSessionHandle();
	}

	public void Dispose()
	{
		_sessionHandle.Dispose();
		_dbContext.Dispose();
		GC.SuppressFinalize(this);
	}

	public void StartTransaction()
	{
		_sessionHandle.StartTransaction();
	}

	public Task StartTransactionAsync()
	{
		_sessionHandle.StartTransaction();
		return Task.CompletedTask;
	}

	public void Commit()
	{
		_sessionHandle.CommitTransaction();
	}

	public async Task CommitAsync(CancellationToken cancellationToken = default)
	{
		await _sessionHandle.CommitTransactionAsync(cancellationToken);
	}

	public void Rollback()
	{
		_sessionHandle.AbortTransaction();
	}

	public async Task RollbackAsync(CancellationToken cancellationToken = default)
	{
		await _sessionHandle.AbortTransactionAsync(cancellationToken);
	}
}
