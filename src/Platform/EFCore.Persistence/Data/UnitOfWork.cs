using EFCore.Persistence.Abstracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.Persistence.Data;

public class UnitOfWork<TContext> : IUnitOfWork<TContext>
    where TContext : DbContext
{
    public UnitOfWork(TContext context)
    {
        this.Context = context;
    }

    public TContext Context { get; }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return await this.Context.Database.BeginTransactionAsync(cancellationToken);
    }

    public IDbContextTransaction BeginTransaction()
    {
        return this.Context.Database.BeginTransaction();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await this.Context.Database.CommitTransactionAsync(cancellationToken);
    }

    public void Commit()
    {
        this.Context.Database.CommitTransaction();
    }
    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await this.Context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public void Rollback()
    {
        this.Context.Database.RollbackTransaction();
    }

    public void Dispose()
    {
        this.Context.Dispose();
        GC.SuppressFinalize(this);
    }
}
