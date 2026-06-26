using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

/// <summary>
/// Represents an error indicating that validation of input data has failed.
/// </summary>
/// <remarks>
/// This error corresponds to HTTP 400 Bad Request and is used when:
/// <list type="bullet">
/// <item><description>Required fields are missing</description></item>
/// <item><description>Data format is invalid (e.g., invalid email, date format)</description></item>
/// <item><description>Values are out of acceptable range</description></item>
/// <item><description>Business validation rules are violated</description></item>
/// </list>
/// 
/// Usage example:
/// <code>
/// var result = Result.Failure(new ValidationError("Email address is required"));
/// </code>
/// </remarks>
public class ValidationError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// <param name="message">A human-readable message describing the validation failure.</param>
    /// <param name="property"></param>
    /// <param name="validationCode"></param>
    /// <param name="code">An optional numeric code overriding the default.</param>
    public ValidationError(string message = DefaultMessage.Validation, string? property = null,
                           string? validationCode = null, int code = (int)DefaultCode.Validation)
        : base(ErrorType.Validation, code, message)
    {
        Property = property;
        ValidationCode = validationCode;
    }


    /// <summary>
    /// 
    /// </summary>
    public string? Property { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ValidationCode { get; set; }
}