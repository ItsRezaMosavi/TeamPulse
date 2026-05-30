using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Resources;

namespace BuildingBlocks.Pagination.Rules;

/// <summary>
/// Domain rule that validates that a page size does not exceed the maximum allowed.
/// </summary>
/// <remarks>
/// This rule prevents excessive data retrieval in a single request by enforcing
/// an upper limit on the page size. This helps protect system resources and
/// ensures reasonable response times.
/// </remarks>
public sealed class PageSizeTooLargeRule(int pageSize, int maxPageSize) : IDomainRule
{
    /// <summary>
    /// Gets the error code returned when this rule is violated.
    /// </summary>
    public string Code => "PAGE_SIZE_TOO_LARGE";

    /// <summary>
    /// Evaluates the rule to determine if the page size is within acceptable limits.
    /// </summary>
    /// <returns>
    /// A <see cref="Clause"/> indicating whether the rule passed or failed.
    /// If failed, includes the error message with both the requested and maximum page sizes.
    /// </returns>
    public Clause Evaluate()
    {
        if (pageSize <= maxPageSize)
            return Clause.Valid();
        return Clause.Invalid(ErrorMessages.PageSizeTooLarge, (nameof(PageRequest.PageSize), pageSize),
                              ("MaxPageSize", maxPageSize));
    }
}
