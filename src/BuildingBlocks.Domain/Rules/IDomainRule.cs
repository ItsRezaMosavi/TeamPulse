namespace BuildingBlocks.Domain.Rules;

/// <summary>
/// Represents a domain business rule that can be evaluated for validity.
/// </summary>
/// <remarks>
/// Domain rules encapsulate business logic that determines whether a particular
/// state or action is valid within the domain. They are used to enforce invariants
/// and business constraints at the domain layer.
/// 
/// Rules should be:
/// <list type="bullet">
/// <item><description>Self-validating - they contain their own logic to determine if broken</description></item>
/// <item><description>Descriptive - providing clear codes and messages when violated</description></item>
/// <item><description>Reusable - can be applied across different parts of the domain</description></item>
/// </list>
/// </remarks>
public interface IDomainRule
{
    /// <summary>
    /// Gets the unique error code associated with this rule violation.
    /// </summary>
    /// <value>A string identifier for the type of rule violation.</value>
    string Code { get; }
    
    /// <summary>
    /// Gets the human-readable message describing the rule violation.
    /// </summary>
    /// <value>A descriptive message explaining why the rule was violated.</value>
    string Message { get; }
    
    /// <summary>
    /// Evaluates whether the business rule is currently violated.
    /// </summary>
    /// <returns><c>true</c> if the rule is broken (violated); otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This method should contain the logic to determine if the current state
    /// violates the business rule. Return true when the rule condition is not met.
    /// </remarks>
    bool IsBroken();
}