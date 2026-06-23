using System.Linq.Expressions;
using BuildingBlocks.Specification.Base;
using BuildingBlocks.Specification.Helpers;

namespace BuildingBlocks.Specification.Composition;

public class AndSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = left.ToExpression();
        var rightExpression = right.ToExpression();

        var parameter = Expression.Parameter(typeof(T), "x");

        var leftBody = new ReplaceParameterVisitor(leftExpression.Parameters[0], parameter).Visit(leftExpression.Body);

        var rightBody =
            new ReplaceParameterVisitor(rightExpression.Parameters[0], parameter).Visit(rightExpression.Body);

        var body = Expression.AndAlso(leftBody, rightBody);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}