using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

/// <summary>
/// Represents an error indicating that a business rule or domain invariant has been violated.
/// </summary>
/// <remarks>
/// This error corresponds to HTTP 409 Conflict and is used when:
/// <list type="bullet">
/// <item><description>A domain-specific business constraint is violated</description></item>
/// <item><description>An operation would break a business invariant</description></item>
/// <item><description>Business logic prevents the requested action</description></item>
/// </list>
/// 
/// Usage example:
/// <code>
/// var result = Result.Failure(new BusinessRuleError("Insufficient funds for this transaction"));
/// </code>
/// </remarks>
public class BusinessRuleError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BusinessRuleError"/> class.
    /// </summary>
    /// <param name="message">A human-readable message describing the rule violation.</param>
    /// <param name="code">An optional numeric code overriding the default.</param>
    public BusinessRuleError(string message = DefaultMessage.BusinessRule, int code = (int)DefaultCode.BusinessRule)
        : base(ErrorType.BusinessRule, code, message)
    {
    }
}
