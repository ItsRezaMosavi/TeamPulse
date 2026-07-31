using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

/// <summary>
/// Represents an error indicating that one or more validation rules have failed.
/// </summary>
/// <remarks>
/// This error is used when input data does not satisfy the validation
/// requirements of an operation.
///
/// Typical scenarios include:
/// <list type="bullet">
/// <item><description>Required values are missing.</description></item>
/// <item><description>Input values have an invalid format.</description></item>
/// <item><description>Values are outside the accepted range.</description></item>
/// <item><description>Custom validation rules are not satisfied.</description></item>
/// </list>
///
/// Usage example:
/// <code>
/// var result = Result.Failure(new ValidationError("Email", "Email address is required"));
/// </code>
/// </remarks>
public class ValidationError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// <param name="property">The name of the property that failed validation.</param>
    /// <param name="message">A human-readable message describing the validation failure.</param>
    /// <param name="validationCode">An optional code identifying the specific validation rule that failed.</param>
    /// <param name="code">An optional application-specific error code overriding the default.</param>
    public ValidationError(string? property = null, string message = DefaultMessage.Validation,
                           string? validationCode = null, string code = DefaultErrorCodes.Validation)
        : base(ErrorType.Validation, code, message)
    {
        Property = property;
        ValidationCode = validationCode;
    }


    /// <summary>
    /// Gets or sets the name of the property that failed validation.
    /// </summary>
    /// <value>The property name, or null if the error is not property-specific.</value>
    public string? Property { get;}

    /// <summary>
    /// Gets or sets the validation code identifying the specific rule that failed.
    /// </summary>
    /// <value>A code representing the validation rule, or null if not specified.</value>
    public string? ValidationCode { get; }
}