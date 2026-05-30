using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Resources;

namespace BuildingBlocks.Pagination.Rules;

/// <summary>
/// Domain rule that validates that a page size is greater than zero.
/// </summary>
/// <remarks>
/// This rule ensures that pagination requests specify a positive number of items per page.
/// Page sizes must be positive integers (1, 2, 3, etc.).
/// </remarks>
public sealed class PageSizeMustBeGreaterThanZeroRule(int pageSize) : IDomainRule
{
    /// <summary>
    /// Gets the error code returned when this rule is violated.
    /// </summary>
    public string Code => "PAGE_SIZE_MUST_BE_GREATER_THAN_ZERO";

    /// <summary>
    /// Evaluates the rule to determine if the page size is valid.
    /// </summary>
    /// <returns>
    /// A <see cref="Clause"/> indicating whether the rule passed or failed.
    /// If failed, includes the error message and the invalid page size value.
    /// </returns>
    public Clause Evaluate()
    {
        if (pageSize > 0)
            return Clause.Valid();

        return Clause.Invalid(ErrorMessages.PageSizeMustBeGreaterThanZero, (nameof(PageRequest.PageSize), pageSize));
    }
}
