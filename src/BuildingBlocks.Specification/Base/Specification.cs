using System.Linq.Expressions;
using BuildingBlocks.Specification.Composition;
using BuildingBlocks.Specification.Contracts;

namespace BuildingBlocks.Specification.Base;

/// <summary>
/// Abstract base class for implementing specifications using the Specification pattern.
/// </summary>
/// <typeparam name="T">The type of entity the specification applies to.</typeparam>
/// <remarks>
/// This class provides:
/// <list type="bullet">
/// <item><description>Compiled delegate caching for efficient in-memory evaluation</description></item>
/// <item><description>Implicit conversion to expression trees for LINQ queries</description></item>
/// <item><description>Fluent composition methods (And, Or, Not) for combining specifications</description></item>
/// </list>
/// 
/// Derived classes must implement <see cref="ToExpression"/> to define the specific criteria.
/// </remarks>
public abstract class Specification<T> : ISpecification<T>
{
    /// <summary>
    /// Cached compiled delegate for efficient repeated evaluations.
    /// </summary>
    private Func<T, bool>? _compiled;

    /// <summary>
    /// Converts the specification to a LINQ expression tree.
    /// </summary>
    /// <returns>An expression representing the specification's criteria.</returns>
    public abstract Expression<Func<T, bool>> ToExpression();

    /// <summary>
    /// Evaluates whether the specified entity satisfies this specification.
    /// </summary>
    /// <param name="entity">The entity to evaluate.</param>
    /// <returns><c>true</c> if the entity satisfies the specification; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// Uses cached compiled delegate for improved performance on repeated calls.
    /// </remarks>
    public bool IsSatisfiedBy(T entity)
    {
        _compiled ??= ToExpression().Compile();
        return _compiled(entity);
    }

    /// <summary>
    /// Implicitly converts a specification to its underlying expression.
    /// </summary>
    /// <param name="spec">The specification to convert.</param>
    /// <returns>The expression tree representing the specification.</returns>
    public static implicit operator Expression<Func<T, bool>>(Specification<T> spec) => spec.ToExpression();

    /// <summary>
    /// Combines this specification with another using logical AND.
    /// </summary>
    /// <param name="other">The specification to combine with.</param>
    /// <returns>A new specification that is satisfied when both this and the other are satisfied.</returns>
    public Specification<T> And(Specification<T> other) => new AndSpecification<T>(this, other);
    
    /// <summary>
    /// Combines this specification with another using logical OR.
    /// </summary>
    /// <param name="other">The specification to combine with.</param>
    /// <returns>A new specification that is satisfied when either this or the other is satisfied.</returns>
    public Specification<T> Or(Specification<T> other) => new OrSpecification<T>(this, other);
    
    /// <summary>
    /// Negates this specification.
    /// </summary>
    /// <returns>A new specification that is satisfied when this one is not satisfied.</returns>
    public Specification<T> Not() => new NotSpecification<T>(this);
}