using SharedCommon.Domain;

namespace EFCore.Persistence.Abstracts;

public interface IAsyncRepository<TEntity, TKey> : 
    IAsyncBaseRepository<TEntity, TKey>, 
    IAsyncReadRepository<TEntity, TKey> 
    where TEntity : class, IEntityBase<TKey>
{
}
