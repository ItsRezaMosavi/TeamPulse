namespace BuildingBlocks.Validation;

public sealed class DomainPolicy
{
    public DomainPolicy(params IDomainRule[] rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        if (rules.Length == 0)
            throw new ArgumentException("Rules must not be empty", nameof(rules));

        foreach (var rule in rules)
            ArgumentNullException.ThrowIfNull(rule);

        Rules = rules.ToArray();
    }

    public IReadOnlyCollection<IDomainRule> Rules { get; init; }


    public IReadOnlyCollection<Clause> Evaluate()
    {
        return Rules.Select(x => x.Evaluate()).Where(x => x.IsInvalid).ToList().AsReadOnly();
    }
}