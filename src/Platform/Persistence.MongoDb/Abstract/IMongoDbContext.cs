using MongoDB.Driver;

namespace Persistence.MongoDb.Abstract;

public interface IMongoDbContext : IDisposable
{
	/// <summary>
	/// Add command to transaction scope
	/// </summary>
	/// <param name="func"></param>
	void AddCommand(Func<Task> func);

	IClientSessionHandle GetSessionHandle();

	/// <summary>
	/// Save changes for commands
	/// </summary>
	/// <returns></returns>
	Task<int> SaveChanges();

	/// <summary>
	/// Get collection from database
	/// </summary>
	/// <typeparam name="TDocument"></typeparam>
	/// <returns></returns>
	IMongoCollection<TDocument> GetCollection<TDocument>() where TDocument : class, IDocument;

	/// <summary>
	/// Get current database
	/// </summary>
	/// <returns></returns>
	IMongoDatabase GetDatabase();

	/// <summary>
	/// Create index for collection when start up project
	/// </summary>
	/// <param name="recreate"></param>
	/// <returns></returns>
	Task InternalCreateIndexesAsync(bool recreate = false);
}
