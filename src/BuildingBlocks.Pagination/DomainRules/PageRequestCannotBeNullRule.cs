using BuildingBlocks.Pagination.Resources;
using BuildingBlocks.Validation;

namespace BuildingBlocks.Pagination.DomainRules;

public sealed class PageRequestCannotBeNullRule(PageRequest? pageRequest) : IDomainRule
{
    public string Code => "PAGE_REQUEST_CANNOT_BE_NULL";

    public Clause Evaluate()
    {
        if (pageRequest is not null)
            return Clause.Valid();

        return Clause.Invalid(ErrorMessages.PageRequestCannotBeNull, nameof(PageRequest));
    }
}