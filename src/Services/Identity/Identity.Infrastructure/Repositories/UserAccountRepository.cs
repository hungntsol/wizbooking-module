using EFCore.Persistence.Data;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;
public class UserAccountRepository : AsyncRepository<UserAccount, ulong, IdentityDataContext>, IUserAccountRepository
{
    public UserAccountRepository(IdentityDataContext context) : base(context)
    {
    }
}
