using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.EfCore.Abstracts;
using SharedCommon.Modules.LoggerAdapter;

namespace Persistence.EfCore.Data;

public class EfCoreAtomicWork<TContext> : IEfCoreAtomicWork<TContext>
	where TContext : DbContext
{
	private readonly ILoggerAdapter<EfCoreAtomicWork<TContext>> _logger;

	public EfCoreAtomicWork(TContext context, ILoggerAdapter<EfCoreAtomicWork<TContext>> logger)
	{
		Context = context;
		_logger = logger;
	}

	public TContext Context { get; }

	public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
	{
		return await Context.Database.BeginTransactionAsync(cancellationToken);
	}

	public IDbContextTransaction BeginTransaction()
	{
		return Context.Database.BeginTransaction();
	}

	public async Task CommitAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			await Context.Database.CommitTransactionAsync(cancellationToken);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "{Message}", e.Message);
			await Context.Database.RollbackTransactionAsync(cancellationToken);
		}
	}

	public void Commit()
	{
		try
		{
			Context.Database.CommitTransaction();
		}
		catch (Exception e)
		{
			_logger.LogError(e, "{Message}", e.Message);
			Context.Database.RollbackTransaction();
		}
	}

	public async Task RollbackAsync(CancellationToken cancellationToken = default)
	{
		await Context.Database.RollbackTransactionAsync(cancellationToken);
	}

	public void Rollback()
	{
		Context.Database.RollbackTransaction();
	}

	public void Dispose()
	{
		Context.Dispose();
		GC.SuppressFinalize(this);
	}
}
