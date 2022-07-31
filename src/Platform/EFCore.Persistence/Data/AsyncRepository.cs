using EFCore.Persistence.Abstracts;
using EFCore.Persistence.Filter;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SharedCommon.Domain;
using System.Linq.Expressions;

namespace EFCore.Persistence.Data;

public class AsyncRepository<TEntity, TKey, TContext> : IAsyncRepository<TEntity, TKey>
    where TContext : DbContext
    where TEntity : class, IEntityBase<TKey>
{
    protected readonly DbSet<TEntity> dbSet;
    protected readonly TContext dbContext;

    public AsyncRepository(TContext context)
    {
        this.dbContext = context;
        this.dbSet = this.dbContext.Set<TEntity>();
    }

    #region Delete

    public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        this.dbSet.Attach(entity).State = EntityState.Deleted;
        return await this.dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeleteBatchAsync(IPredicateDefinition<TEntity> predicateDefinition, CancellationToken cancellationToken = default)
    {
        this.dbSet.RemoveRange(this.dbSet.Where(predicateDefinition.Statement).ToList());
        return await this.dbContext.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Find

    public async Task<TEntity?> FindAndDelete(IPredicateDefinition<TEntity> predicateDefinition, CancellationToken cancellationToken = default)
    {
        var entry = await this.FindOneAsync(predicateDefinition, cancellationToken);
        if (entry is null)
            return null;

        await this.DeleteAsync(entry, cancellationToken);

        return entry;
    }

    public async Task<TEntity?> FindOneAsync(object?[]? keyValues, CancellationToken cancellationToken = default)
    {
        var entities = await this.dbSet.FindAsync(keyValues, cancellationToken)
            .ConfigureAwait(false);
        return entities;
    }

    public Task<TEntity?> FindOneAsync(IPredicateDefinition<TEntity> predicateDefinition, CancellationToken cancellationToken = default)
    => FindOneAsync(predicateDefinition.Statement, cancellationToken);

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    => FindOneAsync(predicate, default, cancellationToken);

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        CancellationToken cancellationToken = default)
    => FindOneAsync(predicate, include, default, cancellationToken);

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        bool asTracking,
        CancellationToken cancellationToken = default)
    => FindOneAsync(predicate, include, asTracking, default, cancellationToken);

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        bool asTracking,
        bool ignoreQueryFilters = true,
        CancellationToken cancellationToken = default)
    => FindOneAsync(predicate, p => p, include, asTracking, ignoreQueryFilters, cancellationToken);

    public async Task<TProject?> FindOneAsync<TProject>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TProject>> selector,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        bool asTracking,
        bool ignoreQueryFilters = true,
        CancellationToken cancellationToken = default)
    {
        return await BuildIQueryable(this.dbSet, include, asTracking, ignoreQueryFilters)
            .Where(predicate)
            .Select(selector)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    #endregion

    #region Insert

    public async Task<TEntity?> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await this.dbSet.AddAsync(entity, cancellationToken);
        var saves = await this.dbContext.SaveChangesAsync(cancellationToken);

        if (saves > 0)
            return entity;

        return null;
    }

    public async Task<TProject?> InsertAsync<TProject>(TEntity entity, CancellationToken cancellationToken = default) where TProject : class
    {
        await this.dbSet.AddAsync(entity, cancellationToken);
        var saves = await this.dbContext.SaveChangesAsync(cancellationToken);

        if (saves > 0)
        {
            return entity.Adapt<TProject>();
        }

        return null;
    }

    public async Task<bool> InsertBatchAsync(IEnumerable<TEntity> entities)
    {
        await this.dbSet.AddRangeAsync(entities);
        var saves = await this.dbContext.SaveChangesAsync();

        return saves > 0;
    }

    #endregion

    #region Update
    public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        this.dbSet.Attach(entity).State = EntityState.Modified;
        var save = await this.dbContext.SaveChangesAsync(cancellationToken);

        return save > 0;
    }

    public async Task<bool> UpdateOneFieldAsync(TEntity entity, Expression<Func<TEntity, object>> update, CancellationToken cancellationToken = default)
    {
        this.dbSet.Attach(entity).Property(update).IsModified = true;
        var save = await this.dbContext.SaveChangesAsync(cancellationToken);

        return save > 0;
    }

    public async Task<bool> Upsert(TEntity entity, CancellationToken cancellationToken = default)
    {
        #region Leave condition

        if (entity is null)
            return false;

        var entry = this.dbContext.Entry(entity);
        var leaveStates = new[] { EntityState.Modified, EntityState.Unchanged, EntityState.Deleted };
        if (leaveStates.Contains(entry.State))
            return false;

        #endregion

        var entityKey = this.dbContext.GetEntityKey(entity);
        if (entityKey is null)
        {
            entry.State = EntityState.Unchanged;
            entityKey = this.dbContext.GetEntityKey(entity);
        }

        if (entityKey?.EntityKeyValues is null ||
            entityKey.EntityKeyValues.Select(ekv => (int)ekv.Value).All(v => v <= 0))
        {
            entry.State = EntityState.Added;
        }

        var save = await this.dbContext.SaveChangesAsync(cancellationToken);
        return save > 0;
    }

    #endregion

    private static IQueryable<TEntity> BuildIQueryable(
        IQueryable<TEntity> queryable,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include,
        bool asTracking,
        bool ignoreFilter
        )
    {
        // tracking
        if (!asTracking)
            queryable = queryable.AsNoTracking();

        // include
        if (include is not null)
            queryable = include(queryable);

        // ignoreFilter
        if (ignoreFilter)
        {
            queryable = queryable.IgnoreQueryFilters();
        }

        return queryable;
    }
}

