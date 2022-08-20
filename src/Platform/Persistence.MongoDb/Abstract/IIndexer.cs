namespace Persistence.MongoDb.Abstract;

public interface IIndexer<T> where T : IDocument
{
	object Ascending(Func<T, object> builderFunc);
	object Descending(Func<T, object> builderFunc);
}
