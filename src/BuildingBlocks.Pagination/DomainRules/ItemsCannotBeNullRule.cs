using BuildingBlocks.Pagination.Resources;
using BuildingBlocks.Validation;

namespace BuildingBlocks.Pagination.DomainRules;

public sealed class ItemsCannotBeNullRule<T>(IReadOnlyList<T>? items) : IDomainRule
{
    public string Code => "ITEMS_CANNOT_BE_NULL";

    public Clause Evaluate()
    {
        if (items is not null)
            return Clause.Valid();
        return Clause.Invalid(ErrorMessages.ItemsCannotBeNull, nameof(PagedResult<T>.Items));
    }
}