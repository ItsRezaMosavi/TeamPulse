namespace BuildingBlocks.Validation;

public sealed class DomainPolicy
{
    private readonly IDomainRule[] _rules;

    public DomainPolicy(params IDomainRule[] rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        if (rules.Length == 0)
            throw new ArgumentException("Rules must not be empty", nameof(rules));

        foreach (var rule in rules)
            ArgumentNullException.ThrowIfNull(rule);

        _rules = rules;
    }


    public IReadOnlyCollection<Clause> Evaluate()
    {
        var clauses = new List<Clause>();
        foreach (var rule in _rules)
        {
            var clause = rule.Evaluate();
            if (clause.IsInvalid)
                clauses.Add(clause);
        }

        return clauses.AsReadOnly();
    }
}