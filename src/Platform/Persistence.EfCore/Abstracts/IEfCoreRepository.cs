using SharedCommon.Commons.Entity;

namespace Persistence.EfCore.Abstracts;

public interface IEfCoreRepository<TEntity, TKey> :
	IEfCoreBaseRepository<TEntity, TKey>,
	IEfCoreReadOnlyRepository<TEntity, TKey>
	where TEntity : class, IEntityBase<TKey>
{
}
