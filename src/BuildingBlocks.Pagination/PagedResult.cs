using BuildingBlocks.Pagination.Policies;
using BuildingBlocks.Validation.Extensions;

namespace BuildingBlocks.Pagination;

public sealed class PagedResult<T>
{
    private PagedResult(IEnumerable<T> items, PageRequest pageRequest, long totalCount)
    {
        Page = pageRequest.Page;
        Items = items.ToArray().AsReadOnly();
        PageSize = pageRequest.PageSize;
        TotalCount = totalCount;
    }

    public static PagedResult<T> Create(IReadOnlyList<T> items, PageRequest pageRequest, long totalCount)
    {
        PagedResultPolicy<T>.Create(items, pageRequest, totalCount).Evaluate().Throw();
        return new(items, pageRequest, totalCount);
    }

    public IReadOnlyList<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public long TotalCount { get; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
    public bool IsEmpty => Items.Count == 0;
}