using System.Linq.Expressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Persistence.EfCore.Abstracts;
using SharedCommon.Commons.Entity;
using SharedCommon.Commons.Exceptions;
using SharedCommon.Commons.PredicateBuilder;

namespace Persistence.EfCore.Data;

public class EfCoreRepository<TEntity, TKey, TContext> : IEfCoreRepository<TEntity, TKey>
	where TContext : DbContext
	where TEntity : class, IEntityBase<TKey>
{
	private readonly IMediator _mediator;
	protected readonly TContext DbContext;
	protected readonly DbSet<TEntity> DbSet;

	public EfCoreRepository(TContext context, IMediator mediator)
	{
		DbContext = context;
		_mediator = mediator;
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

	public async Task<bool> DeleteAsync(TEntity entity,
		bool sendEvent = true,
		CancellationToken cancellationToken = default)
	{
		DbSet.Attach(entity).State = EntityState.Deleted;
		var saves = await DbContext.SaveChangesAsync(cancellationToken) > 0;

		if (!saves)
		{
			return false;
		}

		if (sendEvent)
		{
			await _mediator.Publish(DomainEvent<TEntity>.New(DomainEventAction.Deleted, entity),
				cancellationToken);
		}

		return true;
	}

	public async Task<bool> DeleteBatchAsync(PredicateBuilder<TEntity> predicateBuilder,
		bool sendEvent = true,
		CancellationToken cancellationToken = default)
	{
		DbSet.RemoveRange(DbSet.Where(predicateBuilder.Statement).ToList());
		var saves = await DbContext.SaveChangesAsync(cancellationToken) > 0;

		if (!saves)
		{
			return false;
		}

		if (sendEvent)
		{
			await _mediator.Publish(
				DomainEvent<TEntity>.New($"Batch{nameof(TEntity)}", DomainEventAction.Deleted),
				cancellationToken);
		}

		return true;
	}

	#endregion

	#region Find and delete

	public async Task<TEntity?> FindAndDelete(PredicateBuilder<TEntity> predicateBuilder,
		bool sendEvent = true,
		CancellationToken cancellationToken = default)
	{
		var entry = await FindOneAsync(predicateBuilder, cancellationToken);
		if (entry is null)
		{
			return null;
		}

		await DeleteAsync(entry, sendEvent, cancellationToken);

		return entry;
	}

	public async Task<TEntity?> FindOneAsync(object?[]? keyValues, CancellationToken cancellationToken = default)
	{
		var entity = await DbSet.FindAsync(keyValues, cancellationToken)
			.ConfigureAwait(false);

		return entity;
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
			.FirstOrDefaultAsync(cancellationToken);
	}

	#endregion

	#region Insert

	public async Task<TEntity?> InsertAsync(TEntity entity,
		bool sendEvent = true,
		CancellationToken cancellationToken = default)
	{
		await DbSet.AddAsync(entity, cancellationToken);
		var saves = await DbContext.SaveChangesAsync(cancellationToken);

		if (saves <= 0)
		{
			return null;
		}

		if (sendEvent)
		{
			await _mediator.Publish(DomainEvent<TEntity>.New(DomainEventAction.Created, entity),
				cancellationToken);
		}

		return entity;
	}

	public async Task<bool> InsertBatchAsync(IList<TEntity> entities,
		bool sendEvent = true)
	{
		NullOrEmptyArrayException.ThrowIfNullOrEmpty(entities);

		await DbSet.AddRangeAsync(entities);
		var saves = await DbContext.SaveChangesAsync();

		if (saves <= 0)
		{
			return false;
		}

		if (sendEvent)
		{
			await _mediator.Publish(DomainEvent<TEntity>.New($"Batch{nameof(TEntity)}",
				DomainEventAction.Created,
				entities.ToList()));
		}

		return true;
	}

	#endregion

	#region Update

	public async Task<bool> UpdateAsync(TEntity entity,
		bool sendEvent = true,
		CancellationToken cancellationToken = default)
	{
		DbSet.Attach(entity).State = EntityState.Modified;
		var save = await DbContext.SaveChangesAsync(cancellationToken) > 0;

		if (!save)
		{
			return false;
		}

		if (sendEvent)
		{
			await _mediator.Publish(DomainEvent<TEntity>.New(DomainEventAction.Updated, entity),
				cancellationToken);
		}

		return true;
	}

	public async Task<bool> UpdateOneFieldAsync(TEntity entity,
		Expression<Func<TEntity, object>> update,
		bool sendEvent = true,
		CancellationToken cancellationToken = default)
	{
		DbSet.Attach(entity).Property(update).IsModified = true;
		var save = await DbContext.SaveChangesAsync(cancellationToken) > 0;

		if (!save)
		{
			return false;
		}

		if (sendEvent)
		{
			await _mediator.Publish(DomainEvent<TEntity>.New(DomainEventAction.Updated, entity),
				cancellationToken);
		}

		return true;
	}

	public Task<bool> Upsert(TEntity? entity,
		bool sendEvent = true,
		CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}

	#endregion
}
