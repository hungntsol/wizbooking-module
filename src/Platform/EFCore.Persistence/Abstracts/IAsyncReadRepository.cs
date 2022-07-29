using EFCore.Persistence.Filter;
using Microsoft.EntityFrameworkCore.Query;
using SharedCommon.Domain;
using System.Linq.Expressions;

namespace EFCore.Persistence.Abstracts;

public interface IAsyncReadRepository<TEntity, TKey> 
    where TEntity : class, IEntityIdBase<TKey>
{
    #region Find one

    Task<TEntity?> FindOneAsync(object?[]? keyValues, CancellationToken cancellationToken = default);

    Task<TEntity?> FindOneAsync(IPredicateDefinition<TEntity> predicateDefinition, CancellationToken cancellationToken = default);

    Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FindOneAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FindOneAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        bool asTracking,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FindOneAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        bool asTracking,
        bool ignoreQueryFilters = true,
        CancellationToken cancellationToken = default);

    Task<TProject?> FindOneAsync<TProject>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TProject>> selector,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        bool asTracking,
        bool ignoreQueryFilters = true,
        CancellationToken cancellationToken = default);

    #endregion
}
