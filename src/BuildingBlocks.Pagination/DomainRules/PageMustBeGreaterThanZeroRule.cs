using BuildingBlocks.Pagination.Resources;
using BuildingBlocks.Validation;

namespace BuildingBlocks.Pagination.DomainRules;

public sealed class PageMustBeGreaterThanZeroRule(int page) : IDomainRule
{
    public string Code => "PAGE_MUST_BE_GREATER_THAN_ZERO";

    public Clause Evaluate()
    {
        if (page > 0)
            return Clause.Valid();
        
        return Clause.Invalid(ErrorMessages.PageMustBeGreaterThanZero, (nameof(PageRequest.Page), page));
    }
}