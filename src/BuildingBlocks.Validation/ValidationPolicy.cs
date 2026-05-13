namespace BuildingBlocks.Validation;

public abstract class ValidationPolicy(IEnumerable<IDomainRule> rules)
{
    private readonly DomainPolicy _policy = new(rules.ToArray());

    public IReadOnlyCollection<Clause> Evaluate() => _policy.Evaluate();
}