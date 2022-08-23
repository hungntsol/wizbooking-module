using MongoDB.Driver;

namespace Persistence.MongoDb.Internal;

public class MongoContextConfiguration
{
	public string? Connection { get; set; }
	public string? DatabaseName { get; set; }
	public MongoClientSettings? ClientSettings { get; set; }
}
