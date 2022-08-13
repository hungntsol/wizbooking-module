using SharedCommon.Domain;

namespace EFCore.Persistence.Abstracts;

public interface IEfCoreRepository<TEntity, TKey> :
    IEfCoreBaseRepository<TEntity, TKey>,
    IEfCoreReadRepository<TEntity, TKey>
    where TEntity : class, IEntityBase<TKey>
{
}