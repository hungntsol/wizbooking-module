using EFCore.Persistence.Filter;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EFCore.Persistence.Abstracts
{
    internal interface IAsyncReadRepository<T> where T : class
    {
		#region Find one

		Task<T?> FindOneAsync(object?[]? keyValues, CancellationToken cancellationToken = default);

		Task<T?> FindOneAsync(IPredicateDefinition<T> predicateDefinition, CancellationToken cancellationToken = default);

		Task<T?> FindOneAsync(Expression<Func<T, bool>> predicate,
			CancellationToken cancellationToken = default);

		Task<T?> FindOneAsync(
			Expression<Func<T, bool>> predicate,
			Func<IQueryable<T>, IIncludableQueryable<T, object>>? include,
			CancellationToken cancellationToken = default);

		Task<T?> FindOneAsync(
			Expression<Func<T, bool>> predicate,
			Func<IQueryable<T>, IIncludableQueryable<T, object>>? include,
			bool asTracking,
			CancellationToken cancellationToken = default);

		Task<T?> FindOneAsync(
			Expression<Func<T, bool>> predicate,
			Func<IQueryable<T>, IIncludableQueryable<T, object>>? include,
			bool asTracking,
			bool ignoreQueryFilters = true,
			CancellationToken cancellationToken = default);

		Task<TProject?> FindOneAsync<TProject>(
			Expression<Func<T, bool>> predicate,
			Expression<Func<T, TProject>> selector,
			Func<IQueryable<T>, IIncludableQueryable<T, object>>? include,
			bool asTracking,
			bool ignoreQueryFilters = true,
			CancellationToken cancellationToken = default);

		#endregion
	}
}
