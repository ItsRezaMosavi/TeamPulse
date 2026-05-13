using BuildingBlocks.Pagination.DomainRules;
using BuildingBlocks.Validation;

namespace BuildingBlocks.Pagination.Policies;

public sealed class PageRequestPolicy : ValidationPolicy
{
    private PageRequestPolicy(IEnumerable<IDomainRule> rules) : base(rules)
    {
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
}