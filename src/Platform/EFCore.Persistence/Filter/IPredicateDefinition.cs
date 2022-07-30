using System.Linq.Expressions;

namespace EFCore.Persistence.Filter;

public interface IPredicateDefinition<T> where T : class 
{
    Expression<Func<T, bool>> Statement { get; }

    /// <summary>
    /// Chain two expressions as AND operator
    /// </summary>
    /// <param name="other"></param>
    /// <returns>Statement definition</returns>
    IPredicateDefinition<T> And(Expression<Func<T, bool>> other);

    /// <summary>
    /// Chain two expressions as OR operator
    /// </summary>
    /// <param name="other"></param>
    /// <returns>Statement definition</returns>
    IPredicateDefinition<T> Or(Expression<Func<T, bool>> other);

    /// <summary>
    /// Chain statement with Builder as AND operator
    /// </summary>
    /// <param name="builder"></param>
    /// <returns>Statement definition</returns>
    IPredicateDefinition<T> And(IPredicateBuilder<T> builder);

    /// <summary>
    /// Chain statement with Builder as Or operator
    /// </summary>
    /// <param name="builder"></param>
    /// <returns>Statement definition</returns>
    IPredicateDefinition<T> Or(IPredicateBuilder<T> builder);
}
