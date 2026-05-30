using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Resources;

namespace BuildingBlocks.Pagination.Rules;

/// <summary>
/// Domain rule that validates that the maximum page size is greater than zero.
/// </summary>
/// <remarks>
/// This rule ensures that the configured maximum page size limit is a valid positive value.
/// A non-positive maximum page size would make pagination impossible.
/// </remarks>
public sealed class MaxPageSizeMustBeGreaterThanZero(int maxPageSize) : IDomainRule
{
    /// <summary>
    /// Gets the error code returned when this rule is violated.
    /// </summary>
    public string Code => "MAX_PAGE_SIZE_MUST_BE_GREATER_THAN_ZERO";

    /// <summary>
    /// Evaluates the rule to determine if the maximum page size is valid.
    /// </summary>
    /// <returns>
    /// A <see cref="Clause"/> indicating whether the rule passed or failed.
    /// If failed, includes the error message and the invalid maximum page size value.
    /// </returns>
    public Clause Evaluate()
    {
        if (maxPageSize > 0)
            return Clause.Valid();
        return Clause.Invalid(ErrorMessages.MaxPageSizeMustBeGreaterThanZero, ("MaxPageSize", maxPageSize));
    }
}
