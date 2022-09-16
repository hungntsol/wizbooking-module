namespace SharedCommon.Utilities;

public static class Utils
{
	public static class List
	{
		public static List<T> Empty<T>()
		{
			return Array.Empty<T>().ToList();
		}

		public static bool IsNullOrEmpty<T>(ICollection<T>? target)
		{
			return target is null || target.Count == 0;
		}
	}
}
