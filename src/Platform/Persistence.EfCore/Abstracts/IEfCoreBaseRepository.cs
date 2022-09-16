using System.Linq.Expressions;
using SharedCommon.Commons.Entity;
using SharedCommon.Commons.PredicateBuilder;

namespace Persistence.EfCore.Abstracts;

public interface IEfCoreBaseRepository<TEntity, TKey>
	where TEntity : class, IEntityBase<TKey>
{
	#region Upsert

	/// <summary>
	///     Update or insert an entity
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="sendEvent"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> Upsert(TEntity? entity,
		bool sendEvent = true,
		CancellationToken cancellationToken = default);

	#endregion

	#region Find and delete

	/// <summary>
	///     Find and delete entity with predication definition
	/// </summary>
	/// <param name="predicateDefinition"></param>
	/// <param name="sendEvent"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<TEntity?> FindAndDelete(PredicateBuilder<TEntity> predicateDefinition,
		bool sendEvent = true,
		CancellationToken cancellationToken = default);

	#endregion

	#region Insert

	/// <summary>
	///     Insert one entity async
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="sendEvent"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<TEntity?> InsertAsync(TEntity entity,
		bool sendEvent = true,
		CancellationToken cancellationToken = default);

	/// <summary>
	///     Insert multiple entity
	/// </summary>
	/// <param name="entities"></param>
	/// <param name="sendEvent"></param>
	/// <returns></returns>
	Task<bool> InsertBatchAsync(IList<TEntity> entities, bool sendEvent = true);

	#endregion

	#region Update

	/// <summary>
	///     Update an entity
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="sendEvent"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> UpdateAsync(TEntity entity,
		bool sendEvent = true,
		CancellationToken cancellationToken = default);

	/// <summary>
	///     Update only one field of record
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="update"></param>
	/// <param name="sendEvent"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> UpdateOneFieldAsync(
		TEntity entity,
		Expression<Func<TEntity, object>> update,
		bool sendEvent = true,
		CancellationToken cancellationToken = default);

	#endregion

	#region Delete

	/// <summary>
	///     Delete entity async
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="sendEvent"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> DeleteAsync(TEntity entity,
		bool sendEvent = true,
		CancellationToken cancellationToken = default);

	/// <summary>
	///     Delete multiple entity
	/// </summary>
	/// <param name="predicateDefinition"></param>
	/// <param name="sendEvent"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> DeleteBatchAsync(PredicateBuilder<TEntity> predicateDefinition,
		bool sendEvent = true,
		CancellationToken cancellationToken = default);

	#endregion
}
