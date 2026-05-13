using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

public class NotFoundError(string message = DefaultMessage.NotFound, int code = (int)DefaultCode.NotFound) :
    Error(ErrorType.NotFound, code, message);