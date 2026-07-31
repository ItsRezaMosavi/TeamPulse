using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

/// <summary>
/// Represents an unexpected or unclassified application error.
/// </summary>
/// <remarks>
/// This error is used when an operation fails for a reason that does not
/// fit any of the more specific error categories.
///
/// Typical scenarios include:
/// <list type="bullet">
/// <item><description>An unexpected exception occurs.</description></item>
/// <item><description>An internal operation fails unexpectedly.</description></item>
/// <item><description>No more specific error type accurately describes the failure.</description></item>
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
    /// <param name="code">An optional application-specific error code overriding the default.</param>
    public FailureError(string message = DefaultMessage.Failure, string code = DefaultErrorCodes.Failure)
        : base(ErrorType.Failure, code, message)
    {
    }
}
