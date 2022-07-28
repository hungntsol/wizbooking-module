using EFCore.Persistence.Filter;
using System.Linq.Expressions;

namespace EFCore.Persistence.Abstracts
{
    internal interface IAsyncBaseRepository<T> where T : class
    {
        #region Insert

        /// <summary>
        /// Insert one entity async
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T?> InsertAsync(T entity);
        
        /// <summary>
        /// Insert one entity and project result
        /// </summary>
        /// <typeparam name="TProject"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TProject?> InsertAsync<TProject>(T entity) where TProject : class;

        /// <summary>
        /// Insert multiple entity
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> InsertBatchAsync(IEnumerable<T> entities);

        #endregion

        #region Update

        Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task<bool> UpdateOneFieldAsync(
            T entity,
            Expression<Func<T, object>> update,
            CancellationToken cancellationToken = default);

        #endregion

        #region Upsert
        Task<bool> Upsert(T entity, CancellationToken cancellationToken = default);

        #endregion

        #region Delete

        /// <summary>
        /// Delete entity async
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete multiple entity
        /// </summary>
        /// <param name="predicateDefinition"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteBatchAsync(IPredicateDefinition<T> predicateDefinition,
            CancellationToken cancellationToken = default);

        #endregion

        #region Find and delete

        /// <summary>
        /// Find and delete entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T?> FindAndDelete(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Find and delete entity with predication definition
        /// </summary>
        /// <param name="predicateDefinition"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T?> FindAndDelete(IPredicateDefinition<T> predicateDefinition,
            CancellationToken cancellationToken = default);

        #endregion
    }
}
