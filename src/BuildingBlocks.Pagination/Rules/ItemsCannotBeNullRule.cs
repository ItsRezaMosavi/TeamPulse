using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Resources;

namespace BuildingBlocks.Pagination.Rules;

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