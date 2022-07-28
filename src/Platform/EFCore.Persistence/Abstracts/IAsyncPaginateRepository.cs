using EFCore.Persistence.Paging;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace EFCore.Persistence.Abstracts
{
    internal interface IAsyncPaginateRepository<T> where T : class
    {
        Task<IPaginate<T>> FindPaginateAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool asTracking = false,
            bool ignoreQueryFilters = true,
            int index = 0,
            int size = 20,
            CancellationToken cancellationToken = default);

        Task<IPaginate<TProject>> FindPaginateAsync<TProject>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, TProject>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool asTracking = false,
            bool ignoreQueryFilters = true,
            int index = 0,
            int size = 20,
            CancellationToken cancellationToken = default) where TProject : class;
    }
}
