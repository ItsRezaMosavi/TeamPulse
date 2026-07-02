using System.Linq.Expressions;
using BuildingBlocks.Specification.Base;
using BuildingBlocks.Specification.Helpers;

namespace BuildingBlocks.Specification.Composition;

/// <summary>
/// Represents a specification that combines two specifications using logical OR.
/// </summary>
/// <typeparam name="T">The type of entity the specifications apply to.</typeparam>
/// <remarks>
/// The combined specification is satisfied when either the left or right specification (or both) are satisfied.
/// This is equivalent to the SQL WHERE clause: WHERE (left_condition) OR (right_condition).
/// </remarks>
/// <param name="left">The left specification in the OR operation.</param>
/// <param name="right">The right specification in the OR operation.</param>
public class OrSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>
{
    /// <summary>
    /// Converts the combined OR specification to a LINQ expression tree.
    /// </summary>
    /// <returns>An expression representing (left OR right) condition.</returns>
    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = left.ToExpression();
        var rightExpression = right.ToExpression();

        var parameter = Expression.Parameter(typeof(T), "x");

        var leftBody = new ReplaceParameterVisitor(leftExpression.Parameters[0], parameter).Visit(leftExpression.Body);
        var rightBody =
            new ReplaceParameterVisitor(rightExpression.Parameters[0], parameter).Visit(rightExpression.Body);

        var body = Expression.OrElse(leftBody, rightBody);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}