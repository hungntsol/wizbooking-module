namespace Persistence.MongoDb.Abstract;

public interface IMongoRepository<TDocument> : IMongoReadOnlyRepository<TDocument> where TDocument : IDocument
{
}
