namespace BuildingBlocks.Results;

/// <summary>
/// Defines the categories of errors that can occur during operations.
/// </summary>
/// <remarks>
/// Error types are used to classify errors for appropriate handling and response generation.
/// Each type corresponds to a specific HTTP status code when used in web APIs:
/// <list type="bullet">
/// <item><description><see cref="Failure"/> - 500 Internal Server Error</description></item>
/// <item><description><see cref="Validation"/> - 400 Bad Request</description></item>
/// <item><description><see cref="NotFound"/> - 404 Not Found</description></item>
/// <item><description><see cref="Conflict"/> - 409 Conflict</description></item>
/// <item><description><see cref="Unauthorized"/> - 401 Unauthorized</description></item>
/// <item><description><see cref="Forbidden"/> - 403 Forbidden</description></item>
/// <item><description><see cref="BusinessRule"/> - 409 Conflict (domain rule violation)</description></item>
/// <item><description><see cref="TooManyRequests"/> - 429 Too Many Requests</description></item>
/// <item><description><see cref="Unavailable"/> - 503 Service Unavailable</description></item>
/// <item><description><see cref="Timeout"/> - 408 Request Timeout</description></item>
/// </list>
/// </remarks>
public enum ErrorType
{
    /// <summary>
    /// Represents a general or unknown error that doesn't fit other categories.
    /// Maps to HTTP 500 Internal Server Error.
    /// </summary>
    Failure,
    
    /// <summary>
    /// Represents a validation error where input data fails business rules or format requirements.
    /// Maps to HTTP 400 Bad Request.
    /// </summary>
    Validation,
    
    /// <summary>
    /// Represents an error where a requested resource was not found.
    /// Maps to HTTP 404 Not Found.
    /// </summary>
    NotFound,
    
    /// <summary>
    /// Represents a conflict with the current state of the resource.
    /// Maps to HTTP 409 Conflict.
    /// </summary>
    Conflict,
    
    /// <summary>
    /// Represents an authentication failure where the user is not authenticated.
    /// Maps to HTTP 401 Unauthorized.
    /// </summary>
    Unauthorized,
    
    /// <summary>
    /// Represents an authorization failure where the user lacks required permissions.
    /// Maps to HTTP 403 Forbidden.
    /// </summary>
    Forbidden,

    /// <summary>
    /// Represents a business rule or domain invariant violation.
    /// Maps to HTTP 409 Conflict.
    /// </summary>
    BusinessRule,
    
    /// <summary>
    /// Represents a rate limiting error where too many requests have been made.
    /// Maps to HTTP 429 Too Many Requests.
    /// </summary>
    TooManyRequests,

    /// <summary>
    /// Represents an error where a service or resource is temporarily unavailable.
    /// Maps to HTTP 503 Service Unavailable.
    /// </summary>
    Unavailable,
    
    /// <summary>
    /// Represents an error where an operation timed out.
    /// Maps to HTTP 408 Request Timeout.
    /// </summary>
    Timeout
}
