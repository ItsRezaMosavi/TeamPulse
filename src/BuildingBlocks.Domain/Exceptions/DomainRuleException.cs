using BuildingBlocks.Domain.Rules;

namespace BuildingBlocks.Domain.Exceptions;

/// <summary>
/// Exception thrown when a domain business rule is violated.
/// </summary>
/// <remarks>
/// This exception is raised when an operation attempts to break a domain invariant
/// as defined by an <see cref="IDomainRule"/>. It carries both the error message
/// and the associated error code from the violated rule.
/// </remarks>
public sealed class DomainRuleException(IDomainRule rule) : Exception(rule.Message)
{
    /// <summary>
    /// Gets the error code associated with the violated domain rule.
    /// </summary>
    /// <value>The rule's unique code identifier.</value>
    public string Code => rule.Code;
}