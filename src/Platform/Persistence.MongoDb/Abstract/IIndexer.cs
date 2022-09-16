namespace Persistence.MongoDb.Abstract;

public interface IIndexer<out T> where T : IDocumentEntity
{
	object Ascending(Func<T, object> builderFunc);
	object Descending(Func<T, object> builderFunc);
}
