using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Resources;

namespace BuildingBlocks.Pagination.Rules;

/// <summary>
/// Domain rule that validates that the number of items does not exceed the page size.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
/// <remarks>
/// This rule ensures consistency between the actual number of items returned
/// and the requested page size. A paged result should never contain more items
/// than the specified page size.
/// </remarks>
public sealed class ItemsCountCannotExceedPageSizeRule<T>(IReadOnlyList<T> items, int pageSize) : IDomainRule
{
    /// <summary>
    /// Gets the error code returned when this rule is violated.
    /// </summary>
    public string Code => "ITEMS_COUNT_CANNOT_EXCEED_PAGE_SIZE";

    /// <summary>
    /// Evaluates the rule to determine if the items count is within the page size limit.
    /// </summary>
    /// <returns>
    /// A <see cref="Clause"/> indicating whether the rule passed or failed.
    /// If failed, includes the error message with both the actual item count and the page size.
    /// </returns>
    public Clause Evaluate()
    {
        if (items.Count <= pageSize)
            return Clause.Valid();
        return Clause.Invalid(ErrorMessages.ItemsCountCannotExceedPageSize,
                              (nameof(PagedResult<object>.Items), items.Count),
                              (nameof(PagedResult<object>.PageSize), pageSize));
    }
}
