using Meeting.Domain.Entities;
using MongoDB.Bson.Serialization;

namespace Meeting.Infrastructure.Persistence.Mapping;

public class ScheduleInviteUrlMappingSource
{
	public static void Configure()
	{
		BsonClassMap.RegisterClassMap<ScheduleInviteUrl>(map =>
		{
			map.AutoMap();
			map.SetIgnoreExtraElements(true);
			map.SetIgnoreExtraElementsIsInherited(true);
		});
	}
}
