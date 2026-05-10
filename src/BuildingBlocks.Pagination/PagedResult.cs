namespace BuildingBlocks.Pagination;

public sealed class PagedResult<T>
{
    private PagedResult(IReadOnlyList<T> items, PageRequest pageRequest, int totalCount)
    {
        Page = pageRequest.Page;
        Items = items ?? new List<T>();
        PageSize = pageRequest.PageSize;
        TotalCount = totalCount;
    }

    public static PagedResult<T> Create(IReadOnlyList<T> items, PageRequest pageRequest, int totalCount)
    {
        return new(items, pageRequest, totalCount);
    }

    public IReadOnlyList<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
    public bool IsEmpty => Items.Count == 0;
}