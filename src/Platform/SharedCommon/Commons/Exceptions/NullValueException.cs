using System.Diagnostics.CodeAnalysis;

namespace SharedCommon.Commons.Exceptions;

public static class NullReferenceObjectException
{
	public static void ThrowIfNull([NotNull] object? argument, string? message = null)
	{
		if (argument is null)
		{
			Throw(message);
		}
	}

	[DoesNotReturn]
	private static void Throw(string? message = null)
	{
		throw new NullReferenceException(message);
	}
}
