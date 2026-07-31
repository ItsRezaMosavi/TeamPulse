using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

/// <summary>
/// Represents an error indicating that the current user is not permitted
/// to perform the requested operation.
/// </summary>
/// <remarks>
/// This error is used when the caller is authenticated but does not have
/// sufficient permissions to access a resource or execute an operation.
///
/// Typical scenarios include:
/// <list type="bullet">
/// <item><description>The current user lacks the required permissions.</description></item>
/// <item><description>Access to a protected resource is denied.</description></item>
/// <item><description>The requested operation is not allowed for the current user.</description></item>
/// </list>
///
/// Usage example:
/// <code>
/// var result = Result.Failure(new ForbiddenError("You do not have permission to delete this resource"));
/// </code>
/// </remarks>
public class ForbiddenError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenError"/> class.
    /// </summary>
    /// <param name="message">A human-readable message describing the authorization failure.</param>
    /// <param name="code">An optional application-specific error code overriding the default.</param>
    public ForbiddenError(string message = DefaultMessage.Forbidden, string code = DefaultErrorCodes.Forbidden)
        : base(ErrorType.Forbidden, code, message)
    {
    }
}
