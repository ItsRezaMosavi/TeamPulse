namespace BuildingBlocks.Validation;

public interface IDomainRule
{
    string Code { get; }
    Clause Evaluate();
}