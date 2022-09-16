using System.Linq.Expressions;
using SharedCommon.Utilities.Extensions;

namespace SharedCommon.Commons.PredicateBuilder;

public class PredicateBuilder<T>
{
	public PredicateBuilder()
	{
		Empty();
	}

	public PredicateBuilder(Expression<Func<T, bool>> expression)
	{
		Statement = expression;
	}

	public Expression<Func<T, bool>> Statement { get; private set; } = null!;

	public PredicateBuilder<T> Empty()
	{
		Statement = _ => true;
		return this;
	}

	public PredicateBuilder<T> True(Expression<Func<T, bool>>? expr = default)
	{
		Statement.PipeIfNotNull(_ => expr);
		return this;
	}

	public PredicateBuilder<T> False()
	{
		Statement = _ => false;
		return this;
	}

	public PredicateBuilder<T> And(Expression<Func<T, bool>> other)
	{
		Statement.And(other);

		return this;
	}

	public PredicateBuilder<T> Or(Expression<Func<T, bool>> other)
	{
		Statement.Or(other);
		return this;
	}
}
