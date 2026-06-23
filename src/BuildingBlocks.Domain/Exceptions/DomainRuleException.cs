using BuildingBlocks.Domain.Rules;

namespace BuildingBlocks.Domain.Exceptions;

public sealed class DomainRuleException(IDomainRule rule) : Exception(rule.Message)
{
    public string Code => rule.Code;
}