using Meeting.Domain.Entities;
using MongoDB.Driver;
using Persistence.MongoDb.Data;
using Persistence.MongoDb.Internal;
using SharedCommon.Commons.LoggerAdapter;

namespace Meeting.Infrastructure.Persistence;

public class ScheduleMeetingDbContext : MongoDbContext
{
	public ScheduleMeetingDbContext(MongoContextConfiguration configuration,
		ILoggerAdapter<MongoDbContext> loggerAdapter) : base(configuration, loggerAdapter)
	{
	}

	public override Task InternalCreateIndexesAsync(bool recreate = false)
	{
		Task.WhenAll(CreateScheduleInviteUrlIndexesAsync(recreate),
			CreateScheduleMeetingIndexesAsync(recreate));

		return base.InternalCreateIndexesAsync(recreate);
	}

	internal async Task CreateScheduleInviteUrlIndexesAsync(bool recreate)
	{
		var collection = GetCollection<ScheduleInviteUrl>();

		if (recreate)
		{
			await collection.Indexes.DropAllAsync();
		}

		await collection.Indexes.CreateManyAsync(
			new List<CreateIndexModel<ScheduleInviteUrl>>
			{
				new(Builders<ScheduleInviteUrl>.IndexKeys.Text(doc => doc.GeneratedUrl)),
				new(Builders<ScheduleInviteUrl>.IndexKeys.Descending(doc => doc.CreatorId))
			});
	}

	internal async Task CreateScheduleMeetingIndexesAsync(bool recreate)
	{
		var collection = GetCollection<ScheduleMeeting>();

		if (recreate)
		{
			await collection.Indexes.DropAllAsync();
		}

		await collection.Indexes.CreateManyAsync(
			new List<CreateIndexModel<ScheduleMeeting>>
			{
				new(Builders<ScheduleMeeting>.IndexKeys.Descending(doc => doc.GuestId)),
				new(Builders<ScheduleMeeting>.IndexKeys.Descending(doc => doc.GuestId))
			});
	}
}
