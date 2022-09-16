using SharedCommon.Commons.Entity;

namespace Persistence.MongoDb.Abstract;

/// <summary>
///     Base document of mongo collection
/// </summary>
public interface IDocumentEntity : IDatetimeEntityBase
{
}

/// <summary>
///     Base document props of mongo collection
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IDocumentEntity<TKey> : IEntityBase<TKey>, IDocumentEntity
{
}
