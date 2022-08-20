using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories.Abstracts;
using MediatR;
using Persistence.EfCore.Data;

namespace Identity.Infrastructure.Repositories;

public class UserAccountRepository : EfCoreRepository<UserAccount, ulong, IdentityDataContext>,
	IUserAccountRepository
{
	public UserAccountRepository(IdentityDataContext context, IMediator mediator) : base(context, mediator)
	{
	}
}
