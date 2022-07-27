namespace EFCore.Persistence.Paging
{
    public static class PaginateExtensions
    {
        public static IPaginate<TSource> ToPaging<TSource>(this IEnumerable<TSource> source,
            int index,
            int size,
            int from = 0)
            where TSource : class
        {
            return new Paginate<TSource>(source, index, size, from);
        }

        public static IPaginate<TResult> ToPaging<TSource, TResult>(this IEnumerable<TSource> sources,
            Func<IEnumerable<TSource>, IEnumerable<TResult>> project,
            int index,
            int size,
            int from = 0)
            where TSource : class
            where TResult : class
        {
            return new Paginate<TSource, TResult>(sources, project, index, size, from);
        }
    }
}
