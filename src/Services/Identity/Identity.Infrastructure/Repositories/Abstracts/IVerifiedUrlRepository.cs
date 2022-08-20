using Persistence.EfCore.Abstracts;

namespace Identity.Infrastructure.Repositories.Abstracts;

public interface IVerifiedUrlRepository : IEfCoreRepository<VerifiedUrl, Guid>
{
}
