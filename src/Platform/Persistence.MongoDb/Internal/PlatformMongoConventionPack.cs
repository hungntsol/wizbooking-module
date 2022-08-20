using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace Persistence.MongoDb.Internal;

public static class PlatformMongoConventionPack
{
	public static void Default()
	{
		BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

		var pack = new ConventionPack
		{
			new IgnoreExtraElementsConvention(true),
			new IgnoreIfDefaultConvention(true)
		};

		ConventionRegistry.Register("convention", pack, t => true);
	}
}
