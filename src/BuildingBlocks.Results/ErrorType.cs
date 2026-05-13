namespace BuildingBlocks.Results;

public enum ErrorType
{
    /// <summary>
    /// خطای عمومی و ناشناخته
    /// </summary>
    Failure,
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,

    /// <summary>
    /// نقض قوانین بیزینسی / Domain Rule
    /// </summary>
    BusinessRule,
    TooManyRequests,

    /// <summary>
    /// سرویس یا منبع در دسترس نیست
    /// </summary>
    Unavailable,
    Timeout
}