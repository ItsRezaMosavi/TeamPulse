using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Policies;

namespace BuildingBlocks.Pagination;

/// <summary>
/// Represents a paginated result containing a subset of items along with pagination metadata.
/// </summary>
/// <typeparam name="T">The type of items contained in the result.</typeparam>
/// <remarks>
/// This class encapsulates both the data for the current page and metadata about the overall
/// pagination state, including total count, total pages, and navigation indicators.
/// Validation is performed through the <see cref="Create"/> method using domain policies.
/// </remarks>
public sealed class PagedResult<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResult{T}"/> class.
    /// </summary>
    /// <param name="items">The collection of items for the current page.</param>
    /// <param name="pageRequest">The page request that produced this result.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    private PagedResult(IEnumerable<T> items, PageRequest pageRequest, long totalCount)
    {
        Page = pageRequest.Page;
        Items = items.ToArray().AsReadOnly();
        PageSize = pageRequest.PageSize;
        TotalCount = totalCount;
    }

    /// <summary>
    /// Creates a new <see cref="PagedResult{T}"/> instance with validation.
    /// </summary>
    /// <param name="items">The read-only list of items for the current page.</param>
    /// <param name="pageRequest">The page request that produced this result.</param>
    /// <param name="totalCount">The total number of items across all pages.</param>
    /// <returns>A validated <see cref="PagedResult{T}"/> instance.</returns>
    /// <exception cref="DomainRulesException">Thrown when validation rules are broken.</exception>
    /// <remarks>
    /// This method validates that:
    /// <list type="bullet">
    /// <item><description>Page request is not null</description></item>
    /// <item><description>Items collection is not null</description></item>
    /// <item><description>Total count is not negative</description></item>
    /// <item><description>Items count does not exceed page size</description></item>
    /// </list>
    /// </remarks>
    public static PagedResult<T> Create(IReadOnlyList<T> items, PageRequest pageRequest, long totalCount)
    {
        PagedResultPolicy<T>.Create(items, pageRequest, totalCount).Evaluate().ThrowIfBroken();
        return new(items, pageRequest, totalCount);
    }

    /// <summary>
    /// Gets the collection of items for the current page.
    /// </summary>
    public IReadOnlyList<T> Items { get; }
    
    /// <summary>
    /// Gets the current page number (1-based index).
    /// </summary>
    public int Page { get; }
    
    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int PageSize { get; }
    
    /// <summary>
    /// Gets the total number of items across all pages.
    /// </summary>
    public long TotalCount { get; }
    
    /// <summary>
    /// Gets the total number of pages based on the page size and total count.
    /// </summary>
    /// <value>The calculated total number of pages.</value>
    /// <remarks>
    /// Calculated as: <c>Ceiling(TotalCount / PageSize)</c>
    /// For example, 95 items with page size 10 results in 10 total pages.
    /// </remarks>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    
    /// <summary>
    /// Gets a value indicating whether there is a next page available.
    /// </summary>
    /// <value><c>true</c> if the current page is less than the total pages; otherwise, <c>false</c>.</value>
    public bool HasNextPage => Page < TotalPages;
    
    /// <summary>
    /// Gets a value indicating whether there is a previous page available.
    /// </summary>
    /// <value><c>true</c> if the current page is greater than 1; otherwise, <c>false</c>.</value>
    public bool HasPreviousPage => Page > 1;
    
    /// <summary>
    /// Gets a value indicating whether the result contains no items.
    /// </summary>
    /// <value><c>true</c> if the items collection is empty; otherwise, <c>false</c>.</value>
    public bool IsEmpty => Items.Count == 0;
}
