namespace MongoDb.Persistence.Abstracts;

public interface IMongoRepository<TEntity> : IMongoReadOnlyRepository<TEntity> where TEntity : class
{
}