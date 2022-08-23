using MediatR;
using Meeting.Infrastructure.Repositories.Abstracts;

namespace Meeting.Infrastructure.Repositories;

public class ScheduleMeetingRepository : MongoRepositoryBase<ScheduleMeeting>, IScheduleMeetingRepository
{
	public ScheduleMeetingRepository(IMongoDbContext context, IMediator mediator) : base(context, mediator)
	{
	}
}
