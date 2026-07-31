using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;
/// <summary>
/// Represents an error indicating that a required service or resource
/// is temporarily unavailable.
/// </summary>
/// <remarks>
/// This error is used when an operation cannot be completed because
/// a required dependency is unavailable.
///
/// Typical scenarios include:
/// <list type="bullet">
/// <item><description>An external service is temporarily unavailable.</description></item>
/// <item><description>A required infrastructure component is offline.</description></item>
/// <item><description>The application is temporarily unable to process requests.</description></item>
/// </list>
///
/// Usage example:
/// <code>
/// var result = Result.Failure(new UnavailableError("The notification service is unavailable"));
/// </code>
/// </remarks>
public class UnavailableError(string message = DefaultMessage.Unavailable, string code = DefaultErrorCodes.Unavailable)
    : Error(ErrorType.Unavailable, code, message);