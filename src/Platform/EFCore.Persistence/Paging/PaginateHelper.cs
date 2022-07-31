namespace EFCore.Persistence.Paging;

public static class PaginateHelper
{
    internal static (int, IEnumerable<T>) QueryableSource<T>(IEnumerable<T> source, IReadOnlyCollection<T> enumerable, int index, int from, int size)
    {
        if (source is IQueryable<T> queryable)
        {
            return (queryable.Count(), BuildItemsQuery(queryable, index, from, size));
        }

        return (enumerable.Count, BuildItemsQuery(enumerable, index, from, size));
    }

    internal static int CalculatePages(int total, int size)
    {
        return (int)Math.Ceiling(total / (double)size);
    }

    private static IEnumerable<T> BuildItemsQuery<T>(IEnumerable<T> queryable, int index, int from, int size)
    {
        return queryable.Skip((index - from) * size).Take(size).ToList();
    }

    internal static void HandleFromGreaterThanIndex(ref int index, ref int from)
    {
        // make sure from <= index
        if (from > index)
        {
            from = index;
        }
    }
}

