using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

/// <summary>
/// Represents an error indicating a conflict with the current state of a resource.
/// </summary>
/// <remarks>
/// This error is used when an operation cannot be completed because
/// it conflicts with the current state of the target resource.
///
/// Typical scenarios include:
/// <list type="bullet">
/// <item><description>An entity already exists with the same unique value.</description></item>
/// <item><description>Concurrent modifications conflict with each other.</description></item>
/// <item><description>An operation would result in an invalid resource state.</description></item>
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
    /// <param name="code">An optional application-specific error code overriding the default.</param>
    public ConflictError(string message = DefaultMessage.Conflict, string code = DefaultErrorCodes.Conflict)
        : base(ErrorType.Conflict, code, message)
    {
    }
}
