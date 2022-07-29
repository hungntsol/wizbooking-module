using EFCore.Persistence.Filter;
using SharedCommon.Domain;
using System.Linq.Expressions;

namespace EFCore.Persistence.Abstracts;

public interface IAsyncBaseRepository<TEntity, TKey> 
    where TEntity : class, IEntityIdBase<TKey>
{
    #region Insert

    /// <summary>
    /// Insert one entity async
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TEntity?> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Insert one entity and project result
    /// </summary>
    /// <typeparam name="TProject"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<TProject?> InsertAsync<TProject>(TEntity entity, CancellationToken cancellationToken = default) where TProject : class;

    /// <summary>
    /// Insert multiple entity
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task<bool> InsertBatchAsync(IEnumerable<TEntity> entities);

    #endregion

    #region Update

    Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<bool> UpdateOneFieldAsync(
        TEntity entity,
        Expression<Func<TEntity, object>> update,
        CancellationToken cancellationToken = default);

    #endregion

    #region Upsert
    Task<bool> Upsert(TEntity entity, CancellationToken cancellationToken = default);

        #endregion

        #region Delete

    /// <summary>
    /// Delete entity async
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete multiple entity
    /// </summary>
    /// <param name="predicateDefinition"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> DeleteBatchAsync(IPredicateDefinition<TEntity> predicateDefinition,
        CancellationToken cancellationToken = default);

    #endregion

    #region Find and delete

    /// <summary>
    /// Find and delete entity
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity?> FindAndDelete(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Find and delete entity with predication definition
    /// </summary>
    /// <param name="predicateDefinition"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity?> FindAndDelete(IPredicateDefinition<TEntity> predicateDefinition,
        CancellationToken cancellationToken = default);

    #endregion
}
