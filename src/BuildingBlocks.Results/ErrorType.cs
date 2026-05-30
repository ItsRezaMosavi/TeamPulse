namespace BuildingBlocks.Results;

public enum ErrorType
{
    /// <summary>
    /// General and unknown error
    /// </summary>
    Failure,
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,

    /// <summary>
    /// Business rule / Domain rule violation
    /// </summary>
    BusinessRule,
    TooManyRequests,

    /// <summary>
    /// Service or resource unavailable
    /// </summary>
    Unavailable,
    Timeout
}