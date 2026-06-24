using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

/// <summary>
/// Represents an error indicating that the authenticated user lacks required permissions.
/// </summary>
/// <remarks>
/// This error corresponds to HTTP 403 Forbidden and is used when:
/// <list type="bullet">
/// <item><description>The user is authenticated but not authorized for this action</description></item>
/// <item><description>Access to a resource is denied based on user roles</description></item>
/// <item><description>The user doesn't have sufficient privileges</description></item>
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
    /// <param name="code">An optional numeric code overriding the default.</param>
    public ForbiddenError(string message = DefaultMessage.Forbidden, int code = (int)DefaultCode.Forbidden)
        : base(ErrorType.Forbidden, code, message)
    {
    }
}
