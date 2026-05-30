using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Rules;

namespace BuildingBlocks.Pagination.Policies;

public sealed class PageRequestPolicy
{
    private PageRequestPolicy(IEnumerable<IDomainRule> rules)
    {
        _policy = new DomainPolicy(rules.ToArray());
    }

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

    private readonly DomainPolicy _policy;
    
    public IReadOnlyCollection<Clause> Evaluate() => _policy.Evaluate();

}