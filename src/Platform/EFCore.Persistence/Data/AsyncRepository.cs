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
    where TEntity : class, IEntityIdBase<TKey>
{
    protected readonly DbSet<TEntity> dbSet;
    protected readonly TContext context;

    public AsyncRepository(DbSet<TEntity> dbSet, TContext context)
    {
        this.dbSet = dbSet;
        this.context = context;
    }

    public Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteBatchAsync(IPredicateDefinition<TEntity> predicateDefinition, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #region Find

    public Task<TEntity?> FindAndDelete(TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindAndDelete(IPredicateDefinition<TEntity> predicateDefinition, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindOneAsync(object?[]? keyValues, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindOneAsync(IPredicateDefinition<TEntity> predicateDefinition, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include, bool asTracking, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include, bool asTracking, bool ignoreQueryFilters = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<TProject?> FindOneAsync<TProject>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProject>> selector, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include, bool asTracking, bool ignoreQueryFilters = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Insert

    public async Task<TEntity?> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await this.dbSet.AddAsync(entity, cancellationToken);
        var saves = await this.context.SaveChangesAsync(cancellationToken);

        if (saves > 0)
            return entity;

        return null;
    }

    public async Task<TProject?> InsertAsync<TProject>(TEntity entity, CancellationToken cancellationToken = default) where TProject : class
    {
        await this.dbSet.AddAsync(entity, cancellationToken);
        var saves = await this.context.SaveChangesAsync(cancellationToken);

        if (saves > 0)
        {
            return entity.Adapt<TProject>();
        }

        return null;
    }

    public async Task<bool> InsertBatchAsync(IEnumerable<TEntity> entities)
    {
        await this.dbSet.AddRangeAsync(entities);
        var saves = await this.context.SaveChangesAsync();

        return saves > 0;
    }

    #endregion

    #region Update
    public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        this.dbSet.Attach(entity).State = EntityState.Modified;
        var save = await this.context.SaveChangesAsync(cancellationToken);

        return save > 0;
    }

    public async Task<bool> UpdateOneFieldAsync(TEntity entity, Expression<Func<TEntity, object>> update, CancellationToken cancellationToken = default)
    {
        this.dbSet.Attach(entity).Property(update).IsModified = true;
        var save = await this.context.SaveChangesAsync(cancellationToken);

        return save > 0;
    }

    public async Task<bool> Upsert(TEntity entity, CancellationToken cancellationToken = default)
    {
        #region Leave condition

        if (entity is null)
            return false;

        var entry = this.context.Entry(entity);
        var leaveStates = new[] { EntityState.Modified, EntityState.Unchanged, EntityState.Deleted };
        if (leaveStates.Contains(entry.State))
            return false;

        #endregion

        var entityKey = this.context.GetEntityKey(entity);
        if (entityKey is null)
        {
            entry.State = EntityState.Unchanged;
            entityKey = this.context.GetEntityKey(entity);
        }

        if (entityKey?.EntityKeyValues is null ||
            entityKey.EntityKeyValues.Select(ekv => (int)ekv.Value).All(v => v <= 0))
        {
            entry.State = EntityState.Added;
        }

        var save = await this.context.SaveChangesAsync(cancellationToken);
        return save > 0;
    }

    #endregion
}

