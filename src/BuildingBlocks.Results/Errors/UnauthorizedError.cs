using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

/// <summary>
/// Represents an error indicating that authentication is required or has failed.
/// </summary>
/// <remarks>
/// This error is used when the current caller cannot be authenticated
/// or the provided authentication information is invalid.
///
/// Typical scenarios include:
/// <list type="bullet">
/// <item><description>No authentication credentials were provided.</description></item>
/// <item><description>The provided credentials are invalid or expired.</description></item>
/// <item><description>The current authentication session is no longer valid.</description></item>
/// </list>
///
/// Usage example:
/// <code>
/// var result = Result.Failure(new UnauthorizedError("Invalid username or password"));
/// </code>
/// </remarks>
public class UnauthorizedError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedError"/> class.
    /// </summary>
    /// <param name="message">A human-readable message describing the authentication failure.</param>
    /// <param name="code">An optional application-specific error code overriding the default.</param>
    public UnauthorizedError(string message = DefaultMessage.Unauthorized, string code = DefaultErrorCodes.Unauthorized)
        : base(ErrorType.Unauthorized, code, message)
    {
    }
}
