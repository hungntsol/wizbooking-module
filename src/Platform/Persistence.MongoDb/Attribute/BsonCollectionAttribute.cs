using System.Reflection;
using Persistence.MongoDb.Abstract;

namespace Persistence.MongoDb.Attribute;

/// <summary>
///     This attribute is used to mark collection name
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class BsonCollectionAttribute : System.Attribute
{
	public BsonCollectionAttribute(string collectionName)
	{
		CollectionName = collectionName;
	}

	public string CollectionName { get; init; }
}

/// <summary>
///     Extension for bson entity
/// </summary>
public static class Bson
{
	/// <summary>
	///     Get collection name of entity
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	/// <returns></returns>
	public static string CollectionName<TEntity>() where TEntity : class, IDocumentEntity
	{
		var collectionName = typeof(TEntity).GetCustomAttribute<BsonCollectionAttribute>();
		return collectionName is null ? typeof(TEntity).Name : collectionName.CollectionName;
	}
}
