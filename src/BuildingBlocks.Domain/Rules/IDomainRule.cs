namespace BuildingBlocks.Domain.Rules;

public interface IDomainRule
{
    string Code { get; }
    string Message { get; }
    bool IsBroken();
}