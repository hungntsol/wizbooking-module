using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.EfCore.Abstracts;

namespace Persistence.EfCore.Data;

public class UnitOfWork<TContext> : IUnitOfWork<TContext>
	where TContext : DbContext
{
	public UnitOfWork(TContext context)
	{
		Context = context;
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
		await Context.Database.CommitTransactionAsync(cancellationToken);
	}

	public void Commit()
	{
		Context.Database.CommitTransaction();
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
