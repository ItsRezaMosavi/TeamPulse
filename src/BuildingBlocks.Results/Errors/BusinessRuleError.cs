using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

/// <summary>
/// Represents an error indicating that a business rule
/// or domain invariant has been violated.
/// </summary>
///
/// <remarks>
/// This error is returned when an operation cannot be
/// completed because it violates a business rule.
///
/// Examples:
/// - Insufficient account balance
/// - Order has already been shipped
/// - Product is discontinued
/// </remarks>
public class BusinessRuleError : Error
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BusinessRuleError"/> class.
	/// </summary>
	/// <param name="message">A human-readable message describing the rule violation.</param>
	/// <param name="code">An optional application-specific error code overriding the default.</param>
	public BusinessRuleError(string message = DefaultMessage.BusinessRule, string code = DefaultErrorCodes.BusinessRule)
		: base(ErrorType.BusinessRule, code, message)
	{
	}
}