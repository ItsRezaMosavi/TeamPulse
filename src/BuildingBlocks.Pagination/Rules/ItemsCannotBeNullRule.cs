using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Resources;

namespace BuildingBlocks.Pagination.Rules;

/// <summary>
/// Domain rule that validates that the items collection is not null.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
/// <remarks>
/// This rule ensures that paged results contain a valid items collection.
/// A null items collection would indicate an improperly constructed result.
/// </remarks>
public sealed class ItemsCannotBeNullRule<T>(IReadOnlyList<T>? items) : IDomainRule
{
    /// <summary>
    /// Gets the error code returned when this rule is violated.
    /// </summary>
    public string Code => "ITEMS_CANNOT_BE_NULL";

    /// <summary>
    /// Evaluates the rule to determine if the items collection is not null.
    /// </summary>
    /// <returns>
    /// A <see cref="Clause"/> indicating whether the rule passed or failed.
    /// If failed, includes the error message indicating the null parameter.
    /// </returns>
    public Clause Evaluate()
    {
        if (items is not null)
            return Clause.Valid();
        return Clause.Invalid(ErrorMessages.ItemsCannotBeNull, nameof(PagedResult<T>.Items));
    }
}
