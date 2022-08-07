using EFCore.Persistence.Data;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories.Abstracts;

namespace Identity.Infrastructure.Repositories;

public class VerifiedUrlRepository : AsyncRepository<VerifiedUrl, Guid, IdentityDataContext>, IVerifiedUrlRepository
{
    public VerifiedUrlRepository(IdentityDataContext context) : base(context)
    {
    }
}