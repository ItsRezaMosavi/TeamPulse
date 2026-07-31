namespace BuildingBlocks.Results.Defaults;

internal static class DefaultErrorCodes
{
    public const string Validation = "Validation.Default";
    public const string Unauthorized = "Authentication.Unauthorized";
    public const string Forbidden = "Authorization.Forbidden";
    public const string NotFound = "Common.NotFound";
    public const string Conflict = "Common.Conflict";
    public const string BusinessRule = "Business.RuleViolation";
    public const string TooManyRequests = "Common.TooManyRequests";
    public const string Failure = "Common.Failure";
    public const string Unavailable = "Infrastructure.Unavailable";
    public const string Timeout = "Infrastructure.Timeout";
}