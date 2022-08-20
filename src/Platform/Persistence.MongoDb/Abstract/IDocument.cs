using SharedCommon.Domain;

namespace Persistence.MongoDb.Abstract;

/// <summary>
///     Base document of mongo collection
/// </summary>
public interface IDocument
{
}

/// <summary>
///     Base document props of mongo collection
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IDocument<TKey> : IEntityBase<TKey>, IDocument
{
}
