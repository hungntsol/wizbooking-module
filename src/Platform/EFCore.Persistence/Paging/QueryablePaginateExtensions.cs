using Microsoft.EntityFrameworkCore;

namespace EFCore.Persistence.Paging;

public static class QueryablePaginateExtensions
{
    public static async Task<IPaginate<TModel>> ToPagingAsync<TModel>(this IQueryable<TModel> sources,
        int index,
        int size,
        int from,
        CancellationToken cancellationToken = default)
    where TModel : class
    {
        PaginateHelper.HandleFromGreaterThanIndex(ref index, ref from);

        var total = await sources.CountAsync(cancellationToken).ConfigureAwait(false);
        var items = await sources.Skip((index - from) * size)
            .Take(size).ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var list = new Paginate<TModel>()
        {
            Index = index,
            From = from,
            Size = size,
            Total = total,
            Items = items,
            Pages = PaginateHelper.CalculatePages(total, size)
        };

        return list;
    }
}
