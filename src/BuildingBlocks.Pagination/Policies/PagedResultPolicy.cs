using BuildingBlocks.Pagination.DomainRules;
using BuildingBlocks.Validation;

namespace BuildingBlocks.Pagination.Policies;

public sealed class PagedResultPolicy<T> : ValidationPolicy
{
    private PagedResultPolicy(IEnumerable<IDomainRule> rules) : base(rules)
    {
    }

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
}