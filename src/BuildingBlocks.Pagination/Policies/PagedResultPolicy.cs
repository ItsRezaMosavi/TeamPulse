using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Rules;

namespace BuildingBlocks.Pagination.Policies;

/// <summary>
/// Defines the policy for validating paged result data.
/// </summary>
/// <typeparam name="T">The type of items in the paged result.</typeparam>
/// <remarks>
/// This policy ensures that paged results are constructed with valid data,
/// including non-null collections, valid page requests, and consistent item counts.
/// </remarks>
public sealed class PagedResultPolicy<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PagedResultPolicy{T}"/> class.
    /// </summary>
    /// <param name="rules">The collection of domain rules to apply.</param>
    private PagedResultPolicy(IEnumerable<IDomainRule> rules)
    {
        _policy = new DomainPolicy(rules.ToArray());
    }

    /// <summary>
    /// Creates a new <see cref="PagedResultPolicy{T}"/> with validation rules for the specified paged result parameters.
    /// </summary>
    /// <param name="items">The collection of items to validate.</param>
    /// <param name="pageRequest">The page request to validate.</param>
    /// <param name="totalCount">The total count to validate.</param>
    /// <returns>A new <see cref="PagedResultPolicy{T}"/> instance configured with appropriate validation rules.</returns>
    /// <remarks>
    /// The following rules are applied:
    /// <list type="bullet">
    /// <item><description><see cref="PageRequestCannotBeNullRule"/> - Ensures page request is not null</description></item>
    /// <item><description><see cref="ItemsCannotBeNullRule{T}"/> - Ensures items collection is not null</description></item>
    /// <item><description><see cref="TotalCountCannotBeNegativeRule"/> - Ensures total count is non-negative</description></item>
    /// <item><description><see cref="ItemsCountCannotExceedPageSizeRule{T}"/> - Ensures items count doesn't exceed page size (if page request is not null)</description></item>
    /// </list>
    /// </remarks>
    public static PagedResultPolicy<T> Create(IReadOnlyList<T> items, PageRequest pageRequest, long totalCount)
    {
        var rules = new List<IDomainRule>
        {
            new PageRequestCannotBeNullRule(pageRequest),
            new ItemsCannotBeNullRule<T>(items),
            new TotalCountCannotBeNegativeRule(totalCount)
        };

        if (pageRequest is not null)
            rules.Add(new ItemsCountCannotExceedPageSizeRule<T>(items, pageRequest.PageSize));

        return new PagedResultPolicy<T>(rules);
    }

    /// <summary>
    /// The domain policy containing the validation rules.
    /// </summary>
    private readonly DomainPolicy _policy;

    /// <summary>
    /// Evaluates all validation rules and returns the results.
    /// </summary>
    /// <returns>A collection of <see cref="Clause"/> objects representing the evaluation results of each rule.</returns>
    public IReadOnlyCollection<Clause> Evaluate() => _policy.Evaluate();
}
