using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

public class UnauthorizedError(string message = DefaultMessage.Unauthorized, int code = (int)DefaultCode.Unauthorized)
    : Error(ErrorType.Unauthorized, code, message);