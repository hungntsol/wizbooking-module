using MediatR;
using Meeting.Infrastructure.Repositories.Abstracts;

namespace Meeting.Infrastructure.Repositories;

public class ScheduleInviteUrlRepository : MongoRepositoryBase<ScheduleInviteUrl>, IScheduleInviteUrlRepository
{
	public ScheduleInviteUrlRepository(IMongoDbContext context, IMediator mediator) : base(context, mediator)
	{
	}
}
