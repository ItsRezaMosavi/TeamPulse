using BuildingBlocks.Application.Resources;
using BuildingBlocks.Results.Errors;

namespace BuildingBlocks.Application.Idempotency.Errors;

public class RequestInProgressError()
	: ConflictError(ErrorMessages.RequestInProgressError, IdempotencyErrorCodes.RequestInProgress);