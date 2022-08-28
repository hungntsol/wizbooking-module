using MediatR;
using Meeting.Infrastructure.Repositories.Abstracts;

namespace Meeting.Infrastructure.Repositories;

public class HostUserSupplyingRepository : MongoRepositoryBase<HostUserSupplying>, IHostUserSupplyingRepository
{
	public HostUserSupplyingRepository(IMongoDbContext context, IMediator mediator) : base(context, mediator)
	{
	}
}
