using System.Diagnostics.CodeAnalysis;

namespace SharedCommon.Exceptions;

public static class NullOrEmptyArrayException
{
	public static void ThrowIfNullOrEmpty([NotNull] object? argument, string? message = null)
	{
		if (argument is null || ((Array)argument).Length == 0)
		{
			Throw(message);
		}
	}

	[DoesNotReturn]
	private static void Throw(string? message = null)
	{
		throw new ArgumentNullException(message);
	}
}
