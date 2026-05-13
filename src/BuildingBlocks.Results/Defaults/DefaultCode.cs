namespace BuildingBlocks.Results.Defaults;

internal enum DefaultCode
{
    Validation = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    BusinessRule = 422,
    TooManyRequests = 429,
    Failure = 500,
    Unavailable = 503,
    Timeout = 504,
}