using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;
/// <summary>
/// Represents an error indicating that an operation did not complete
/// within the expected time.
/// </summary>
/// <remarks>
/// This error is used when an operation exceeds its allowed execution time.
///
/// Typical scenarios include:
/// <list type="bullet">
/// <item><description>A database operation times out.</description></item>
/// <item><description>An external service does not respond in time.</description></item>
/// <item><description>A long-running operation exceeds its timeout limit.</description></item>
/// </list>
///
/// Usage example:
/// <code>
/// var result = Result.Failure(new TimeoutError("The operation timed out"));
/// </code>
/// </remarks>
public class TimeoutError(string message = DefaultMessage.Timeout, string code = DefaultErrorCodes.Timeout)
    : Error(ErrorType.Timeout, code, message);