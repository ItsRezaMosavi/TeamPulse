using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Resources;

namespace BuildingBlocks.Pagination.Rules;

public sealed class PageSizeMustBeGreaterThanZeroRule(int pageSize) : IDomainRule
{
    public string Code => "PAGE_SIZE_MUST_BE_GREATER_THAN_ZERO";

    public Clause Evaluate()
    {
        if (pageSize > 0)
            return Clause.Valid();

        return Clause.Invalid(ErrorMessages.PageSizeMustBeGreaterThanZero, (nameof(PageRequest.PageSize), pageSize));
    }
}