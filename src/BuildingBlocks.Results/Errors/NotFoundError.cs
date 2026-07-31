using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

/// <summary>
/// Represents an error indicating that a requested resource could not be found.
/// </summary>
/// <remarks>
/// This error is used when an operation references a resource that does not
/// exist or is no longer available.
///
/// Typical scenarios include:
/// <list type="bullet">
/// <item><description>An entity with the specified identifier does not exist.</description></item>
/// <item><description>A referenced resource cannot be located.</description></item>
/// <item><description>A dependency required to complete the operation is missing.</description></item>
/// </list>
///
/// Usage example:
/// <code>
/// var result = Result.Failure(new NotFoundError("User with ID 123 was not found"));
/// </code>
/// </remarks>
public class NotFoundError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundError"/> class.
    /// </summary>
    /// <param name="message">A human-readable message describing the error.</param>
    /// <param name="code">An optional application-specific error code overriding the default.</param>
    public NotFoundError(string message = DefaultMessage.NotFound, string code = DefaultErrorCodes.NotFound)
        : base(ErrorType.NotFound, code, message)
    {
    }
}
