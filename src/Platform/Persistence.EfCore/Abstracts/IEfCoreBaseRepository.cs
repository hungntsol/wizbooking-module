using System.Linq.Expressions;
using SharedCommon.Domain;
using SharedCommon.PredicateBuilder;

namespace Persistence.EfCore.Abstracts;

public interface IEfCoreBaseRepository<TEntity, TKey>
	where TEntity : class, IEntityBase<TKey>
{
	#region Upsert

	/// <summary>
	///     Update or insert an entity
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> Upsert(TEntity? entity,
		CancellationToken cancellationToken = default);

	#endregion

	#region Find and delete

	/// <summary>
	///     Find and delete entity with predication definition
	/// </summary>
	/// <param name="predicateDefinition"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<TEntity?> FindAndDelete(PredicateBuilder<TEntity> predicateDefinition,
		CancellationToken cancellationToken = default);

	#endregion

	#region Insert

	/// <summary>
	///     Insert one entity async
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<TEntity?> InsertAsync(TEntity entity,
		CancellationToken cancellationToken = default);

	/// <summary>
	///     Insert one entity and project result
	/// </summary>
	/// <typeparam name="TProject"></typeparam>
	/// <param name="entity"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<TProject?> InsertAsync<TProject>(TEntity entity,
		CancellationToken cancellationToken = default)
		where TProject : class;

	/// <summary>
	///     Insert multiple entity
	/// </summary>
	/// <param name="entities"></param>
	/// <returns></returns>
	Task<bool> InsertBatchAsync(IEnumerable<TEntity> entities);

	#endregion

	#region Update

	/// <summary>
	///     Update an entity
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

	/// <summary>
	///     Update only one field of record
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="update"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> UpdateOneFieldAsync(
		TEntity entity,
		Expression<Func<TEntity, object>> update,
		CancellationToken cancellationToken = default);

	#endregion

	#region Delete

	/// <summary>
	///     Delete entity async
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

	/// <summary>
	///     Delete multiple entity
	/// </summary>
	/// <param name="predicateDefinition"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> DeleteBatchAsync(PredicateBuilder<TEntity> predicateDefinition,
		CancellationToken cancellationToken = default);

	#endregion
}
