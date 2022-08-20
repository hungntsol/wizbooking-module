using MongoDB.Driver;

namespace Persistence.MongoDb.Abstract;

public interface IMongoDbContext : IDisposable
{
	void AddCommand(Func<Task> func);
	Task<int> SaveChanges();
	IMongoCollection<TDocument> GetCollection<TDocument>() where TDocument : class, IDocument;
	IMongoDatabase GetDatabase();
}
