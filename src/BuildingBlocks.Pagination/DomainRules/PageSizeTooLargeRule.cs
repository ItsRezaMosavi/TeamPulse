using BuildingBlocks.Pagination.Resources;
using BuildingBlocks.Validation;

namespace BuildingBlocks.Pagination.DomainRules;

public sealed class PageSizeTooLargeRule(int pageSize, int maxPageSize) : IDomainRule
{
    public string Code => "PAGE_SIZE_TOO_LARGE";

    public Clause Evaluate()
    {
        if (pageSize <= maxPageSize)
            return Clause.Valid();
        return Clause.Invalid(ErrorMessages.PageSizeTooLarge, (nameof(PageRequest.PageSize), pageSize),
                              ("MaxPageSize", maxPageSize));
    }
}