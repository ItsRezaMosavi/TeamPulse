using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

/// <summary>
/// Represents a general or unknown error that doesn't fit other specific categories.
/// </summary>
/// <remarks>
/// This error corresponds to HTTP 500 Internal Server Error and is used when:
/// <list type="bullet">
/// <item><description>An unexpected exception occurs</description></item>
/// <item><description>The error doesn't match any specific error type</description></item>
/// <item><description>A generic failure needs to be reported</description></item>
/// </list>
/// 
/// Usage example:
/// <code>
/// var result = Result.Failure(new FailureError("An unexpected error occurred"));
/// </code>
/// </remarks>
public class FailureError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FailureError"/> class.
    /// </summary>
    /// <param name="message">A human-readable message describing the failure.</param>
    /// <param name="code">An optional numeric code overriding the default.</param>
    public FailureError(string message = DefaultMessage.Failure, int code = (int)DefaultCode.Failure)
        : base(ErrorType.Failure, code, message)
    {
    }
}
