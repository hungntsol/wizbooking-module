namespace Persistence.EfCore.Paging;

public interface IPaginate<out TModel> where TModel : class
{
	int From { get; set; }
	int Index { get; set; }
	int Size { get; set; }
	int Total { get; set; }
	int Pages { get; set; }
	IEnumerable<TModel> Items { get; }
	bool HasNext { get; }
	bool HasPrevious { get; }
}
