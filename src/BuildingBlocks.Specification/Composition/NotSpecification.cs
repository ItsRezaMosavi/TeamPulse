using System.Linq.Expressions;
using BuildingBlocks.Specification.Base;

namespace BuildingBlocks.Specification.Composition;

public class NotSpecification<T>(Specification<T> spec) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var expression = spec.ToExpression();

        var notExpression = Expression.Not(expression.Body);

        return Expression.Lambda<Func<T, bool>>(notExpression, expression.Parameters);
    }
}