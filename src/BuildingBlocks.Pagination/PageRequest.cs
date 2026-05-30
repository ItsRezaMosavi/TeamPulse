using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Policies;

namespace BuildingBlocks.Pagination;

public sealed class PageRequest
{
    public const int DefaultPageSize = 10;
    public const int DefaultMaxPageSize = 100;

    private PageRequest(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public static PageRequest Create(int page, int pageSize = DefaultPageSize, int maxPageSize = DefaultMaxPageSize)
    {
        PageRequestPolicy.Create(page, pageSize, maxPageSize).Evaluate().ThrowIfBroken();
        return new PageRequest(page, pageSize);
    }

    public int Page { get; }
    public int PageSize { get; }

    public int Skip => (Page - 1) * PageSize;
}