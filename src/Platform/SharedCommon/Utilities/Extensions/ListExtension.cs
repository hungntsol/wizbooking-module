namespace SharedCommon.Utilities.Extensions;

public static class ListExtension
{
	public static bool IsNullOrEmpty(this IList<string>? list)
	{
		return list is not null && list.IsEmpty();
	}

	public static bool IsEmpty(this IList<string> list)
	{
		return list.Count == 0;
	}
}
