using System.Linq.Expressions;

namespace EFCore.Persistence.Filter.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> AndAlso<T>(
            this Expression<Func<T, bool>> main,
            Expression<Func<T, bool>> other)
        {
            var parameter = VisitLeftAndRight(main, other, out var left, out var right);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }

        public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> main,
        Expression<Func<T, bool>> other)
        {
            var parameter = VisitLeftAndRight(main, other, out var left, out var right);

            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left, right), parameter);
        }


        private static ParameterExpression VisitLeftAndRight<T>(Expression<Func<T, bool>> main,
        Expression<Func<T, bool>> join,
        out Expression left, out Expression right)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(main.Parameters[0], parameter);
            left = leftVisitor.Visit(main.Body);

            var rightVisitor = new ReplaceExpressionVisitor(join.Parameters[0], parameter);
            right = rightVisitor.Visit(join.Body);
            return parameter;
        }
    }
}
