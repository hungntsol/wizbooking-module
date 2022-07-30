using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.Persistence.Abstracts;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Begin a transaction scope
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Flush all saveChanges have been made in scope
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CommitAsync(CancellationToken cancellationToken = default);

    IDbContextTransaction BeginTransaction();

    void Commit();

    Task RollbackAsync(CancellationToken cancellationToken = default);

    void Rollback();
}

public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : DbContext
{
    /// <summary>
    /// Get DbContext of application
    /// </summary>
    TContext Context { get; }
}
