using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

public class ForbiddenError(string message = DefaultMessage.Forbidden, int code = (int)DefaultCode.Forbidden) :
    Error(ErrorType.Forbidden, code, message);