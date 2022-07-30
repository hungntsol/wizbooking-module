using System.Linq.Expressions;

namespace EFCore.Persistence.Filter;

public interface IPredicateBuilder<T> where T : class
{
    Expression<Func<T, bool>> Statement { get; }

    /// <summary>
    /// Create an empty (always true) predicate statement
    /// </summary>
    /// <returns></returns>
    IPredicateBuilder<T> Empty();
    
    /// <summary>
    /// Create a chain can include multiple Where statement
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    IPredicateBuilder<T> Where(Expression<Func<T, bool>> predicate);
}
