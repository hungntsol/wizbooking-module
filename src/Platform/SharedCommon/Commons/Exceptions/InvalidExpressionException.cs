using System.Linq.Expressions;

namespace SharedCommon.Commons.Exceptions;

public static class InvalidExpressionException
{
	public static void ThrowIfInvalid<T>(Expression<Func<T, object>> expression)
	{
		ArgumentNullException.ThrowIfNull(expression, nameof(expression));

		if (expression.Body.NodeType == ExpressionType.Parameter)
		{
			throw new ArgumentException("Cannot generate property path from lambda parameter");
		}
	}
}
