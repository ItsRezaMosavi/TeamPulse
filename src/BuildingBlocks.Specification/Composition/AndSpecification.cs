using System.Linq.Expressions;
using BuildingBlocks.Specification.Base;
using BuildingBlocks.Specification.Helpers;

namespace BuildingBlocks.Specification.Composition;

/// <summary>
/// Represents a specification that combines two specifications using logical AND.
/// </summary>
/// <typeparam name="T">The type of entity the specifications apply to.</typeparam>
/// <remarks>
/// The combined specification is satisfied only when both the left and right specifications are satisfied.
/// This is equivalent to the SQL WHERE clause: WHERE (left_condition) AND (right_condition).
/// </remarks>
/// <param name="left">The left specification in the AND operation.</param>
/// <param name="right">The right specification in the AND operation.</param>
public class AndSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>
{
    /// <summary>
    /// Converts the combined AND specification to a LINQ expression tree.
    /// </summary>
    /// <returns>An expression representing (left AND right) condition.</returns>
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