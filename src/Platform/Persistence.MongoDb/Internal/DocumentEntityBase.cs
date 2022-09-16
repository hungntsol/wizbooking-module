using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Persistence.MongoDb.Abstract;
using SharedCommon.Commons.Entity;

namespace Persistence.MongoDb.Internal;

public class DocumentEntityBase<TKey> : SupportPayloadEvent, IDocumentEntity<TKey>, ISupportPayloadEvent
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public TKey Id { get; set; } = default!;

	public DateTime CreatedAt { get; set; }
	public DateTime ModifiedAt { get; set; }
}
