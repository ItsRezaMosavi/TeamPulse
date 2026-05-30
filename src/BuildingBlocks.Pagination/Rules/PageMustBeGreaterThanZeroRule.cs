using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Resources;

namespace BuildingBlocks.Pagination.Rules;

/// <summary>
/// Domain rule that validates that a page number is greater than zero.
/// </summary>
/// <remarks>
/// This rule ensures that pagination requests use valid 1-based page numbers.
/// Page numbers must be positive integers (1, 2, 3, etc.).
/// </remarks>
public sealed class PageMustBeGreaterThanZeroRule(int page) : IDomainRule
{
    /// <summary>
    /// Gets the error code returned when this rule is violated.
    /// </summary>
    public string Code => "PAGE_MUST_BE_GREATER_THAN_ZERO";

    /// <summary>
    /// Evaluates the rule to determine if the page number is valid.
    /// </summary>
    /// <returns>
    /// A <see cref="Clause"/> indicating whether the rule passed or failed.
    /// If failed, includes the error message and the invalid page value.
    /// </returns>
    public Clause Evaluate()
    {
        if (page > 0)
            return Clause.Valid();

        return Clause.Invalid(ErrorMessages.PageMustBeGreaterThanZero, (nameof(PageRequest.Page), page));
    }
}
