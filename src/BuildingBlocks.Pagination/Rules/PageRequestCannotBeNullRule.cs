using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Resources;

namespace BuildingBlocks.Pagination.Rules;

/// <summary>
/// Domain rule that validates that a page request is not null.
/// </summary>
/// <remarks>
/// This rule ensures that paged results are created with a valid page request.
/// A null page request would make it impossible to determine pagination parameters.
/// </remarks>
public sealed class PageRequestCannotBeNullRule(PageRequest? pageRequest) : IDomainRule
{
    /// <summary>
    /// Gets the error code returned when this rule is violated.
    /// </summary>
    public string Code => "PAGE_REQUEST_CANNOT_BE_NULL";

    /// <summary>
    /// Evaluates the rule to determine if the page request is not null.
    /// </summary>
    /// <returns>
    /// A <see cref="Clause"/> indicating whether the rule passed or failed.
    /// If failed, includes the error message indicating the null parameter.
    /// </returns>
    public Clause Evaluate()
    {
        if (pageRequest is not null)
            return Clause.Valid();

        return Clause.Invalid(ErrorMessages.PageRequestCannotBeNull, nameof(PageRequest));
    }
}
