using EFCore.Persistence.Data;
using Identity.Infrastructure.Persistence;
using Identity.Infrastructure.Repositories.Abstracts;
using MediatR;

namespace Identity.Infrastructure.Repositories;

public class VerifiedUrlRepository : EfCoreRepository<VerifiedUrl, Guid, IdentityDataContext>, IVerifiedUrlRepository
{
    public VerifiedUrlRepository(IdentityDataContext context, IMediator mediator) : base(context, mediator)
    {
    }
}
