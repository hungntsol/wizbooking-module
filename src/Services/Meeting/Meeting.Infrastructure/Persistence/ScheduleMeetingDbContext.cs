using SharedCommon.Modules.LoggerAdapter;

namespace Meeting.Infrastructure.Persistence;

public class ScheduleMeetingDbContext : MongoDbContext
{
	public ScheduleMeetingDbContext(MongoContextConfiguration contextConfiguration,
		ILoggerAdapter<MongoDbContext> logger) : base(contextConfiguration, logger)
	{
	}

	public override Task InternalCreateIndexesAsync(bool recreate = false)
	{
		Task.WhenAll(CreateScheduleInviteUrlIndexesAsync(recreate),
			CreateScheduleMeetingIndexesAsync(recreate),
			CreateUserHostServiceIndexesAsync(recreate));

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

	internal async Task CreateUserHostServiceIndexesAsync(bool recreate)
	{
		var collection = GetCollection<HostUserSupplying>();
		if (recreate)
		{
			await collection.Indexes.DropAllAsync();
		}

		await collection.Indexes.CreateManyAsync(new List<CreateIndexModel<HostUserSupplying>>
		{
			new(Builders<HostUserSupplying>.IndexKeys.Text(doc => doc.Name)),
			new(Builders<HostUserSupplying>.IndexKeys.Text(doc => doc.HostId))
		});
	}
}
