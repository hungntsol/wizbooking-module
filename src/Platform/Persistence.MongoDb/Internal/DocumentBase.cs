using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Persistence.MongoDb.Abstract;
using SharedCommon.Domain;

namespace Persistence.MongoDb.Internal;

public class DocumentBase<TKey> : SupportPayloadEvent, IDocument<TKey>, ISupportPayloadEvent
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public TKey Id { get; set; } = default!;

	public DateTime CreatedAt { get; set; }
	public DateTime ModifiedAt { get; set; }
}
