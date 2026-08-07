using System.Linq.Expressions;

namespace BuildingBlocks.Specification.Contracts;

public interface IQuerySpecification<T>
{
	/// <summary>
	///     Defines a specification for querying entities with support for filtering, ordering, paging, and includes.
	/// </summary>
	Expression<Func<T, bool>>? Criteria { get; }

	/// <summary>
	///     Defines a specification for querying entities with support for filtering, ordering, paging, and includes.
	/// </summary>
	Expression<Func<T, object>>? OrderBy { get; }


	/// <summary>
	///     Defines a specification for querying entities with support for filtering, ordering, paging, and includes.
	/// </summary>
	public bool IsAscending { get; }

	/// <summary>
	///     Defines a specification for querying entities with support for filtering, ordering, paging, and includes.
	/// </summary>
	IReadOnlyList<Expression<Func<T, object>>> Includes { get; }

	/// <summary>
	///     Defines a specification for querying entities with support for filtering, ordering, paging, and includes.
	/// </summary>
	int Skip { get; }

	/// <summary>
	///     Defines a specification for querying entities with support for filtering, ordering, paging, and includes.
	/// </summary>
	int Take { get; }

	/// <summary>
	///     Defines a specification for querying entities with support for filtering, ordering, paging, and includes.
	/// </summary>
	bool IsPagingEnabled { get; }

	/// <summary>
	///     Defines a specification for querying entities with support for filtering, ordering, paging, and includes.
	/// </summary>
	bool IsSplitQuery { get; }

	/// <summary>
	///     Determines whether the entities should be tracked by the DbContext.
	///     Default is false (AsNoTracking) for optimal read performance.
	/// </summary>
	bool IsTrackingEnabled { get; }
}