using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Rules;

namespace BuildingBlocks.Pagination.Policies;

/// <summary>
/// Defines the policy for validating page request parameters.
/// </summary>
/// <remarks>
/// This policy ensures that pagination requests meet the required constraints
/// before they are used to query data. It validates page numbers, page sizes,
/// and maximum page size limits.
/// </remarks>
public sealed class PageRequestPolicy
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PageRequestPolicy"/> class.
    /// </summary>
    /// <param name="rules">The collection of domain rules to apply.</param>
    private PageRequestPolicy(IEnumerable<IDomainRule> rules)
    {
        _policy = new DomainPolicy(rules.ToArray());
    }

    /// <summary>
    /// Creates a new <see cref="PageRequestPolicy"/> with validation rules for the specified parameters.
    /// </summary>
    /// <param name="page">The page number to validate.</param>
    /// <param name="pageSize">The page size to validate.</param>
    /// <param name="maxPageSize">The maximum allowed page size.</param>
    /// <returns>A new <see cref="PageRequestPolicy"/> instance configured with appropriate validation rules.</returns>
    /// <remarks>
    /// The following rules are applied:
    /// <list type="bullet">
    /// <item><description><see cref="PageMustBeGreaterThanZeroRule"/> - Ensures page is positive</description></item>
    /// <item><description><see cref="PageSizeMustBeGreaterThanZeroRule"/> - Ensures page size is positive</description></item>
    /// <item><description><see cref="PageSizeTooLargeRule"/> - Ensures page size doesn't exceed maximum</description></item>
    /// <item><description><see cref="MaxPageSizeMustBeGreaterThanZero"/> - Ensures max page size is positive</description></item>
    /// </list>
    /// </remarks>
    public static PageRequestPolicy Create(int page, int pageSize, int maxPageSize)
    {
        var rules = new List<IDomainRule>()
        {
            new PageMustBeGreaterThanZeroRule(page),
            new PageSizeMustBeGreaterThanZeroRule(pageSize),
            new PageSizeTooLargeRule(pageSize, maxPageSize),
            new MaxPageSizeMustBeGreaterThanZero(maxPageSize)
        };
        return new PageRequestPolicy(rules);
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
