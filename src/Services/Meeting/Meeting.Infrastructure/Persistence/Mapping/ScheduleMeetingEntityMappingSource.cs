using MongoDB.Bson.Serialization;

namespace Meeting.Infrastructure.Persistence.Mapping;

public class ScheduleMeetingEntityMappingSource
{
	public static void Configure()
	{
		BsonClassMap.RegisterClassMap<ScheduleInviteUrl>(map =>
		{
			map.AutoMap();
			map.SetIgnoreExtraElements(true);
			map.SetIgnoreExtraElementsIsInherited(true);
		});

		BsonClassMap.RegisterClassMap<ScheduleMeeting>(map =>
		{
			map.AutoMap();
			map.AutoMap();
			map.SetIgnoreExtraElements(true);
			map.SetIgnoreExtraElementsIsInherited(true);
		});

		BsonClassMap.RegisterClassMap<HostUserSupplying>(map =>
		{
			map.AutoMap();
			map.AutoMap();
			map.SetIgnoreExtraElements(true);
			map.SetIgnoreExtraElementsIsInherited(true);
		});
	}
}
