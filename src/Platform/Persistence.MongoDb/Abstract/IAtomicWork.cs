namespace Persistence.MongoDb.Abstract;

public interface IAtomicWork : IDisposable
{
	void StartTransaction();
	Task StartTransactionAsync();
	void Commit();
	Task CommitAsync(CancellationToken cancellationToken = default);
	void Rollback();
	Task RollbackAsync(CancellationToken cancellationToken = default);
}
