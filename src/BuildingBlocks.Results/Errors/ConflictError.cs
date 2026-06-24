using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

/// <summary>
/// Represents an error indicating a conflict with the current state of a resource.
/// </summary>
/// <remarks>
/// This error corresponds to HTTP 409 Conflict and is used when:
/// <list type="bullet">
/// <item><description>An entity already exists with the same unique value</description></item>
/// <item><description>Concurrent modifications conflict with each other</description></item>
/// <item><description>An operation would create an invalid state</description></item>
/// </list>
/// 
/// Usage example:
/// <code>
/// var result = Result.Failure(new ConflictError("A user with this email already exists"));
/// </code>
/// </remarks>
public class ConflictError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictError"/> class.
    /// </summary>
    /// <param name="message">A human-readable message describing the conflict.</param>
    /// <param name="code">An optional numeric code overriding the default.</param>
    public ConflictError(string message = DefaultMessage.Conflict, int code = (int)DefaultCode.Conflict)
        : base(ErrorType.Conflict, code, message)
    {
    }
}
