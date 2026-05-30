using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Resources;

namespace BuildingBlocks.Pagination.Rules;

/// <summary>
/// Domain rule that validates that the total count is not negative.
/// </summary>
/// <remarks>
/// This rule ensures that the total count of items across all pages is a valid
/// non-negative number. A negative total count would be logically inconsistent.
/// </remarks>
public sealed class TotalCountCannotBeNegativeRule(long totalCount) : IDomainRule
{
    /// <summary>
    /// Gets the error code returned when this rule is violated.
    /// </summary>
    public string Code => "TOTAL_COUNT_CANNOT_BE_NEGATIVE";

    /// <summary>
    /// Evaluates the rule to determine if the total count is non-negative.
    /// </summary>
    /// <returns>
    /// A <see cref="Clause"/> indicating whether the rule passed or failed.
    /// If failed, includes the error message and the invalid total count value.
    /// </returns>
    public Clause Evaluate()
    {
        if (totalCount >= 0)
            return Clause.Valid();

        return Clause.Invalid(ErrorMessages.TotalCountCannotBeNegative, (nameof(PagedResult<object>.TotalCount),
                                                                         totalCount));
    }
}
