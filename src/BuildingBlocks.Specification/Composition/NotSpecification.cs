using System.Linq.Expressions;
using BuildingBlocks.Specification.Base;

namespace BuildingBlocks.Specification.Composition;

/// <summary>
/// Represents a specification that negates another specification using logical NOT.
/// </summary>
/// <typeparam name="T">The type of entity the specification applies to.</typeparam>
/// <remarks>
/// The negated specification is satisfied when the original specification is NOT satisfied.
/// This is equivalent to the SQL WHERE clause: WHERE NOT (original_condition).
/// </remarks>
/// <param name="spec">The specification to negate.</param>
public class NotSpecification<T>(Specification<T> spec) : Specification<T>
{
    /// <summary>
    /// Converts the negated specification to a LINQ expression tree.
    /// </summary>
    /// <returns>An expression representing NOT (original condition).</returns>
    public override Expression<Func<T, bool>> ToExpression()
    {
        var expression = spec.ToExpression();

        var notExpression = Expression.Not(expression.Body);

        return Expression.Lambda<Func<T, bool>>(notExpression, expression.Parameters);
    }
}