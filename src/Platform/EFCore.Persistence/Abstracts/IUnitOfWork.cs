using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.Persistence.Abstracts;

public interface IUnitOfWork
{
    /// <summary>
    /// Begin a transaction scope
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IDbContextTransaction> StartTransaction(CancellationToken cancellationToken = default);

    /// <summary>
    /// Flush all saveChanges have been made in scope
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IDbContextTransaction> CommitTransaction(CancellationToken cancellationToken = default);
}
