﻿using EFCore.Persistence.Data;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories.Abstracts;

namespace Identity.Infrastructure.Repositories;

public class UserAccountRepository : EfCoreRepository<UserAccount, ulong, IdentityDataContext>,
    IUserAccountRepository
{
    public UserAccountRepository(IdentityDataContext context) : base(context)
    {
    }
}
