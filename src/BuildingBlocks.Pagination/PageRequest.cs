using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Policies;

namespace BuildingBlocks.Pagination;

/// <summary>
/// Represents a request for paginated data, specifying which page to retrieve and how many items per page.
/// </summary>
/// <remarks>
/// This class uses value object patterns with a private constructor to ensure that only valid page requests
/// can be created. Validation is performed through the <see cref="Create"/> method using domain policies.
/// </remarks>
public sealed class PageRequest
{
    /// <summary>
    /// The default number of items per page if not specified.
    /// </summary>
    public const int DefaultPageSize = 10;
    
    /// <summary>
    /// The maximum allowed page size to prevent excessive data retrieval in a single request.
    /// </summary>
    public const int DefaultMaxPageSize = 100;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="PageRequest"/> class.
    /// </summary>
    /// <param name="page">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    private PageRequest(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    /// <summary>
    /// Creates a new <see cref="PageRequest"/> instance with validation.
    /// </summary>
    /// <param name="page">The page number to retrieve (must be greater than zero).</param>
    /// <param name="pageSize">The number of items per page (defaults to <see cref="DefaultPageSize"/>).</param>
    /// <param name="maxPageSize">The maximum allowed page size (defaults to <see cref="DefaultMaxPageSize"/>).</param>
    /// <returns>A validated <see cref="PageRequest"/> instance.</returns>
    /// <exception cref="DomainRulesException">Thrown when validation rules are broken.</exception>
    /// <remarks>
    /// This method validates that:
    /// <list type="bullet">
    /// <item><description>Page number is greater than zero</description></item>
    /// <item><description>Page size is greater than zero</description></item>
    /// <item><description>Page size does not exceed the maximum allowed</description></item>
    /// <item><description>Maximum page size is greater than zero</description></item>
    /// </list>
    /// </remarks>
    public static PageRequest Create(int page, int pageSize = DefaultPageSize, int maxPageSize = DefaultMaxPageSize)
    {
        PageRequestPolicy.Create(page, pageSize, maxPageSize).Evaluate().ThrowIfBroken();
        return new PageRequest(page, pageSize);
    }

    /// <summary>
    /// Gets the page number to retrieve (1-based index).
    /// </summary>
    public int Page { get; }
    
    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Calculates the number of items to skip based on the current page and page size.
    /// </summary>
    /// <value>The number of items to skip for database queries (zero-based offset).</value>
    /// <remarks>
    /// Formula: <c>(Page - 1) * PageSize</c>
    /// For example, page 3 with page size 10 returns Skip = 20.
    /// </remarks>
    public int Skip => (Page - 1) * PageSize;
}
