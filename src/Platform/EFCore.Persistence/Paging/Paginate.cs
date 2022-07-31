namespace EFCore.Persistence.Paging;

public class Paginate<TModel> : IPaginate<TModel> where TModel : class
{
	public Paginate(IEnumerable<TModel> source, int index, int size, int from)
	{
		var enumerable = source as TModel[] ?? source.ToArray();

		PaginateHelper.HandleFromGreaterThanIndex(ref index, ref from);

		Size = size;
		Index = index;
		From = from;

		(Total, Items) = PaginateHelper.QueryableSource(source, enumerable, index, from, size);
		Pages = PaginateHelper.CalculatePages(Total, Size);
	}

	public Paginate()
	{
		Items = ArraySegment<TModel>.Empty;
	}

	public int From { get; set; }
	public int Index { get; set; }
	public int Size { get; set; }
	public int Total { get; set; }
	public int Pages { get; set; }
	public IEnumerable<TModel> Items { get; set; }
	public bool HasNext => Index - From > 0;
	public bool HasPrevious => Index - From + 1 < Pages;
}

public class Paginate<TSource, TResult> : IPaginate<TResult> where TSource : class where TResult : class
{
	public Paginate(IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> projection,
		int index, int size, int from)
	{
		var enumerable = source as TSource[] ?? source.ToArray();

		PaginateHelper.HandleFromGreaterThanIndex(ref index, ref from);

		Size = size;
		Index = index;
		From = from;

		(Total, var items) = PaginateHelper.QueryableSource(source, enumerable, index, from, size);
		Items = new List<TResult>(projection(items));
		Pages = PaginateHelper.CalculatePages(Total, Size);
	}

	public int From { get; set; }
	public int Index { get; set; }
	public int Size { get; set; }
	public int Total { get; set; }
	public int Pages { get; set; }
	public IEnumerable<TResult> Items { get; }
	public bool HasNext => Index - From > 0;
	public bool HasPrevious => Index - From + 1 < Pages;
}