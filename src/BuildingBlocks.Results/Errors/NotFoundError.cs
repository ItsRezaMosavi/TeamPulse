using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

/// <summary>
/// Represents an error indicating that a requested resource was not found.
/// </summary>
/// <remarks>
/// This error corresponds to HTTP 404 Not Found and is used when:
/// <list type="bullet">
/// <item><description>An entity with the specified ID does not exist</description></item>
/// <item><description>A requested endpoint or route is not available</description></item>
/// <item><description>A referenced resource cannot be located</description></item>
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
    /// <param name="code">An optional numeric code overriding the default.</param>
    public NotFoundError(string message = DefaultMessage.NotFound, int code = (int)DefaultCode.NotFound)
        : base(ErrorType.NotFound, code, message)
    {
    }
}
