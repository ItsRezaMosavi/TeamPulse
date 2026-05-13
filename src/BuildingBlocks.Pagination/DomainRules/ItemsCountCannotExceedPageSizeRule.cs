using BuildingBlocks.Pagination.Resources;
using BuildingBlocks.Validation;

namespace BuildingBlocks.Pagination.DomainRules;

public sealed class ItemsCountCannotExceedPageSizeRule<T>(IReadOnlyList<T> items, int pageSize) : IDomainRule
{
    public string Code => "ITEMS_COUNT_CANNOT_EXCEED_PAGE_SIZE";

    public Clause Evaluate()
    {
        if (items.Count <= pageSize)
            return Clause.Valid();
        return Clause.Invalid(ErrorMessages.ItemsCountCannotExceedPageSize,
                              (nameof(PagedResult<object>.Items), items.Count),
                              (nameof(PagedResult<object>.PageSize), pageSize));
    }
}