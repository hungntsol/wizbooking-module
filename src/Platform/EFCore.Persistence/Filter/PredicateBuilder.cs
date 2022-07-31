using System.Linq.Expressions;

namespace EFCore.Persistence.Filter;

public class PredicateBuilder<T> : IPredicateBuilder<T> where T : class
{
    public Expression<Func<T, bool>> Statement { get; private set; } = null!;

    public IPredicateBuilder<T> Empty()
    {
        Statement = q => true;
        return this;
    }
    public IPredicateBuilder<T> Where(Expression<Func<T, bool>> predicate)
    {
        Statement = predicate;
        return this;
    }
}
