using System.Linq.Expressions;

namespace BuildingBlocks.Specification.Contracts;

/// <summary>
/// Defines a specification that encapsulates business rules for filtering and evaluating entities.
/// </summary>
/// <typeparam name="T">The type of entity the specification applies to.</typeparam>
/// <remarks>
/// The Specification pattern allows you to:
/// <list type="bullet">
/// <item><description>Encapsulate query logic for reuse across the application</description></item>
/// <item><description>Combine specifications using boolean operations (And, Or, Not)</description></item>
/// <item><description>Separate business rules from infrastructure concerns</description></item>
/// </list>
/// 
/// A specification can be converted to an expression tree for database queries or
/// compiled to a delegate for in-memory evaluation.
/// </remarks>
internal interface ISpecification<T>
{
	/// <summary>
	///     Converts the specification to a LINQ expression tree.
	/// </summary>
	/// <returns>An expression that can be used in LINQ queries.</returns>
	/// <remarks>
	///     This method is typically used by ORMs like Entity Framework to translate
	///     the specification into SQL or other query languages.
	/// </remarks>
	public Expression<Func<T, bool>> ToExpression();

	/// <summary>
	///     Evaluates whether the specified entity satisfies this specification.
	/// </summary>
	/// <param name="entity">The entity to evaluate.</param>
	/// <returns><c>true</c> if the entity satisfies the specification; otherwise, <c>false</c>.</returns>
	/// <remarks>
	///     This method is useful for in-memory validation of entities against business rules
	///     without executing a database query.
	/// </remarks>
	public bool IsSatisfiedBy(T entity);
}