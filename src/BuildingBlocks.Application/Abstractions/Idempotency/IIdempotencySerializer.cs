using BuildingBlocks.Results;

namespace BuildingBlocks.Application.Abstractions.Idempotency;

public interface IIdempotencySerializer
{
	string Serialize<T>(Result<T> result);

	Result<T> Deserialize<T>(string value);
}