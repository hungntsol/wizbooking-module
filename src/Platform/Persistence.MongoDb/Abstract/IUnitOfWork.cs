namespace Persistence.MongoDb.Abstract;

/// <summary>
/// Mongo Unit of work
/// </summary>
public interface IUnitOfWork : IDisposable
{
	Task<bool> Commit();
}
