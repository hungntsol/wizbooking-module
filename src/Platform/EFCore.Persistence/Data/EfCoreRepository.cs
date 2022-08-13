using System.Linq.Expressions;
using EFCore.Persistence.Abstracts;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedCommon.Domain;
using SharedCommon.PredicateBuilder;

namespace EFCore.Persistence.Data;

public class EfCoreRepository<TEntity, TKey, TContext> : IEfCoreRepository<TEntity, TKey>
    where TContext : DbContext
    where TEntity : class, IEntityBase<TKey>
{
    protected readonly TContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    public EfCoreRepository(TContext context)
    {
        DbContext = context;
        DbSet = DbContext.Set<TEntity>();
    }

    private static IQueryable<TEntity> BuildIQueryable(
        IQueryable<TEntity> queryable,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        bool asTracking,
        bool ignoreFilter
    )
    {
        // tracking
        if (!asTracking)
        {
            queryable = queryable.AsNoTracking();
        }

        // include
        if (include is not null)
        {
            queryable = include(queryable);
        }

        // ignoreFilter
        if (ignoreFilter)
        {
            queryable = queryable.IgnoreQueryFilters();
        }

        return queryable;
    }

    #region Delete

    public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Attach(entity).State = EntityState.Deleted;
        return await DbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteBatchAsync(PredicateBuilder<TEntity> predicateBuilder,
        CancellationToken cancellationToken = default)
    {
        DbSet.RemoveRange(DbSet.Where(predicateBuilder.Statement).ToList());
        return await DbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Find

    public async Task<TEntity?> FindAndDelete(PredicateBuilder<TEntity> predicateBuilder,
        CancellationToken cancellationToken = default)
    {
        var entry = await FindOneAsync(predicateBuilder, cancellationToken);
        if (entry is null)
        {
            return null;
        }

        await DeleteAsync(entry, cancellationToken);

        return entry;
    }

    public async Task<TEntity?> FindOneAsync(object?[]? keyValues, CancellationToken cancellationToken = default)
    {
        var entities = await DbSet.FindAsync(keyValues, cancellationToken)
            .ConfigureAwait(false);
        return entities;
    }

    public Task<TEntity?> FindOneAsync(PredicateBuilder<TEntity> predicateBuilder,
        CancellationToken cancellationToken = default)
    {
        return FindOneAsync(predicateBuilder.Statement, cancellationToken);
    }

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return FindOneAsync(predicate, default, cancellationToken);
    }

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        CancellationToken cancellationToken = default)
    {
        return FindOneAsync(predicate, include, default, cancellationToken);
    }

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        bool asTracking,
        CancellationToken cancellationToken = default)
    {
        return FindOneAsync(predicate, include, asTracking, default, cancellationToken);
    }

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        bool asTracking,
        bool ignoreQueryFilters = true,
        CancellationToken cancellationToken = default)
    {
        return FindOneAsync(predicate, p => p, include, asTracking, ignoreQueryFilters, cancellationToken);
    }

    public async Task<TProject?> FindOneAsync<TProject>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TProject>> selector,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        bool asTracking,
        bool ignoreQueryFilters = true,
        CancellationToken cancellationToken = default)
    {
        return await BuildIQueryable(DbSet, include, asTracking, ignoreQueryFilters)
            .Where(predicate)
            .Select(selector)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    #endregion

    #region Insert

    public async Task<TEntity?> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        var saves = await DbContext.SaveChangesAsync(cancellationToken);

        if (saves > 0)
        {
            return entity;
        }

        return null;
    }

    public async Task<TProject?> InsertAsync<TProject>(TEntity entity, CancellationToken cancellationToken = default)
        where TProject : class
    {
        await DbSet.AddAsync(entity, cancellationToken);
        var saves = await DbContext.SaveChangesAsync(cancellationToken);

        if (saves > 0)
        {
            return entity.Adapt<TProject>();
        }

        return null;
    }

    public async Task<bool> InsertBatchAsync(IEnumerable<TEntity> entities)
    {
        await DbSet.AddRangeAsync(entities);
        var saves = await DbContext.SaveChangesAsync();

        return saves > 0;
    }

    #endregion

    #region Update

    public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Attach(entity).State = EntityState.Modified;
        var save = await DbContext.SaveChangesAsync(cancellationToken);

        return save > 0;
    }

    public async Task<bool> UpdateOneFieldAsync(TEntity entity, Expression<Func<TEntity, object>> update,
        CancellationToken cancellationToken = default)
    {
        DbSet.Attach(entity).Property(update).IsModified = true;
        var save = await DbContext.SaveChangesAsync(cancellationToken);

        return save > 0;
    }

    public async Task<bool> Upsert(TEntity? entity, CancellationToken cancellationToken = default)
    {
        #region Leave condition

        if (entity is null)
        {
            return false;
        }

        var entry = DbContext.Entry(entity);
        var leaveStates = new[] { EntityState.Modified, EntityState.Unchanged, EntityState.Deleted };
        if (leaveStates.Contains(entry.State))
        {
            return false;
        }

        #endregion

        var entityKey = DbContext.GetEntityKey(entity);
        if (entityKey is null)
        {
            entry.State = EntityState.Unchanged;
            entityKey = DbContext.GetEntityKey(entity);
        }

        if (entityKey?.EntityKeyValues is null ||
            entityKey.EntityKeyValues.Select(ekv => (int)ekv.Value).All(v => v <= 0))
        {
            entry.State = EntityState.Added;
        }

        var save = await DbContext.SaveChangesAsync(cancellationToken);
        return save > 0;
    }

    #endregion
}
