namespace BuildingBlocks.Domain.Rules;

public interface IDomainRule
{
    string Code { get; }
    Clause Evaluate();
}