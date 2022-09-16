using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories.Abstracts;
using MediatR;
using Persistence.EfCore.Data;

namespace Identity.Infrastructure.Repositories;

public class VerifiedUrlRepository : EfCoreRepository<VerifiedUrl, Guid, AppDbContext>, IVerifiedUrlRepository
{
	public VerifiedUrlRepository(AppDbContext context, IMediator mediator) : base(context, mediator)
	{
	}
}
