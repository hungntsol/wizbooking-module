using SharedCommon.Commons.Domain;

namespace Persistence.EfCore.Abstracts;

public interface IEfCoreRepository<TEntity, TKey> :
	IEfCoreBaseRepository<TEntity, TKey>,
	IEfCoreReadRepository<TEntity, TKey>
	where TEntity : class, IEntityBase<TKey>
{
	IEfCoreRepository<TEntity, TKey> DispatchEvent(bool enable);
}
