using BuildingBlocks.Domain.Rules;
using BuildingBlocks.Pagination.Rules;

namespace BuildingBlocks.Pagination.Policies;

public sealed class PagedResultPolicy<T>
{
    private PagedResultPolicy(IEnumerable<IDomainRule> rules)
    {
        _policy = new DomainPolicy(rules.ToArray());
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


    private readonly DomainPolicy _policy;

    public IReadOnlyCollection<Clause> Evaluate() => _policy.Evaluate();
}