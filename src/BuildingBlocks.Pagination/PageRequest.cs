namespace BuildingBlocks.Pagination;

public sealed class PageRequest
{
    private const int MaxPageSize = 100;
    private const int DefaultPageSize = 10;
    private const int DefaultPage = 1;
    
    public PageRequest(int page = DefaultPage, int pageSize = DefaultPageSize)
    {
        Page = page > 0 ? page : DefaultPage;
        PageSize = pageSize <= 0 ? DefaultPageSize : Math.Min(pageSize, MaxPageSize);
    }

    public int Page { get; }
    public int PageSize { get; }

    public int Skip => (Page - 1) * PageSize;
}