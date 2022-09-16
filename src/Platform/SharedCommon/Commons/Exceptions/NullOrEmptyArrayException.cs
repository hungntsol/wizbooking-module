using System.Diagnostics.CodeAnalysis;

namespace SharedCommon.Commons.Exceptions;

public static class NullOrEmptyArrayException
{
	public static void ThrowIfNullOrEmpty([NotNull] object? argument)
	{
		if (argument is null || ((Array)argument).Length == 0)
		{
			Throw($"{nameof(argument)} is an empty list or array");
		}
	}

	[DoesNotReturn]
	private static void Throw(string message)
	{
		throw new Exception(message);
	}
}
