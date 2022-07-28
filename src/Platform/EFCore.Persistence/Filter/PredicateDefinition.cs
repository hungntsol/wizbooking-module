using EFCore.Persistence.Filter.Extensions;
using System.Linq.Expressions;

namespace EFCore.Persistence.Filter
{
    public class PredicateDefinition<T> : IPredicateDefinition<T> where T : class
    {
        private Expression<Func<T, bool>> _statement;
        private IPredicateBuilder<T> _builder;

        #region Ctor

        public PredicateDefinition(Expression<Func<T, bool>> statement)
        {
            _statement = statement;
        }

        public PredicateDefinition(IPredicateBuilder<T> builder)
        {
            this._builder = builder;
            this._statement = builder.Statement;
        }

        public PredicateDefinition()
        {
            _statement = new PredicateBuilder<T>().Empty().Statement;
        }

        #endregion

        public Expression<Func<T, bool>> Statement => this._statement;

        public IPredicateDefinition<T> And(Expression<Func<T, bool>> other)
        {
            this._statement = this._statement.AndAlso(other);
            return this;
        }

        public IPredicateDefinition<T> Or(Expression<Func<T, bool>> other)
        {
            this._statement = this._statement.Or(other);
            return this;
        }

        public IPredicateDefinition<T> And(IPredicateBuilder<T> builder)
        {
            this._statement = this._statement.AndAlso(builder.Statement);
            return this;
        }

        public IPredicateDefinition<T> Or(IPredicateBuilder<T> builder)
        {
            this._statement = this._statement.Or(builder.Statement);
            return this;
        }
    }
}
