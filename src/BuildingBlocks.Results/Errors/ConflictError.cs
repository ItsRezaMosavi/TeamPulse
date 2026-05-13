using BuildingBlocks.Results.Defaults;

namespace BuildingBlocks.Results.Errors;

public class ConflictError(string message = DefaultMessage.Conflict, int code = (int)DefaultCode.Conflict) :
    Error(ErrorType.Conflict, code, message);