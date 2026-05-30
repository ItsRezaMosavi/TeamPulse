using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Resources;

namespace BuildingBlocks.Pagination.Rules;

public sealed class MaxPageSizeMustBeGreaterThanZero(int maxPageSize) : IDomainRule
{
    public string Code => "MAX_PAGE_SIZE_MUST_BE_GREATER_THAN_ZERO";

    public Clause Evaluate()
    {
        if (maxPageSize > 0)
            return Clause.Valid();
        return Clause.Invalid(ErrorMessages.MaxPageSizeMustBeGreaterThanZero, ("MaxPageSize", maxPageSize));
    }
}