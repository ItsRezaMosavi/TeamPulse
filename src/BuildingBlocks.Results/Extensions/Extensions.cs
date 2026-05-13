using System.Net;

namespace BuildingBlocks.Results.Extensions;

public static class Extensions
{
    public static HttpStatusCode AsHttpStatusCode(this ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Failure => HttpStatusCode.InternalServerError,
            ErrorType.Validation => HttpStatusCode.BadRequest,
            ErrorType.NotFound => HttpStatusCode.NotFound,
            ErrorType.Conflict => HttpStatusCode.Conflict,
            ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
            ErrorType.Forbidden => HttpStatusCode.Forbidden,
            ErrorType.BusinessRule => HttpStatusCode.BadRequest,
            ErrorType.TooManyRequests => HttpStatusCode.TooManyRequests,
            ErrorType.Unavailable => HttpStatusCode.ServiceUnavailable,
            ErrorType.Timeout => HttpStatusCode.RequestTimeout,
            _ => HttpStatusCode.InternalServerError
        };
    }

}