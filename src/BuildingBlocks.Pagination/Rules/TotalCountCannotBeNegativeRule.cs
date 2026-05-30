using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Resources;

namespace BuildingBlocks.Pagination.Rules;

public sealed class TotalCountCannotBeNegativeRule(long totalCount) : IDomainRule
{
    public string Code => "TOTAL_COUNT_CANNOT_BE_NEGATIVE";

    public Clause Evaluate()
    {
        if (totalCount >= 0)
            return Clause.Valid();

        return Clause.Invalid(ErrorMessages.TotalCountCannotBeNegative, (nameof(PagedResult<object>.TotalCount),
                                                                         totalCount));
    }
}