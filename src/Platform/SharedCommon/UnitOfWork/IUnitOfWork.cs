namespace SharedCommon.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
	void StartTransaction();
	Task StartTransactionAsync();
	void Commit();
	Task CommitAsync(CancellationToken cancellationToken = default);
	void Rollback();
	Task RollbackAsync(CancellationToken cancellationToken = default);
}
