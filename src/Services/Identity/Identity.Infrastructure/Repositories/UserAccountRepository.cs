using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories.Abstracts;
using MediatR;
using Persistence.EfCore.Data;

namespace Identity.Infrastructure.Repositories;

public class UserAccountRepository : EfCoreRepository<UserAccount, ulong, AppDbContext>,
	IUserAccountRepository
{
	public UserAccountRepository(AppDbContext context, IMediator mediator) : base(context, mediator)
	{
	}
}
