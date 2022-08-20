using Persistence.EfCore.Abstracts;

namespace Identity.Infrastructure.Repositories.Abstracts;

public interface IUserAccountRepository : IEfCoreRepository<UserAccount, ulong>
{
}
