using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

/// <summary>
/// Represents an error indicating that too many requests have been made.
/// </summary>
/// <remarks>
/// This error is used when an operation cannot be processed because
/// a rate limit or request quota has been exceeded.
///
/// Typical scenarios include:
/// <list type="bullet">
/// <item><description>A client exceeds the allowed request rate.</description></item>
/// <item><description>An operation is temporarily throttled.</description></item>
/// <item><description>A configured usage limit has been reached.</description></item>
/// </list>
///
/// Usage example:
/// <code>
/// var result = Result.Failure(new TooManyRequestsError("Rate limit exceeded"));
/// </code>
/// </remarks>
public class TooManyRequestsError(
    string message = DefaultMessage.TooManyRequests,
    string code = DefaultErrorCodes.TooManyRequests)
    : Error(ErrorType.TooManyRequests, code, message);