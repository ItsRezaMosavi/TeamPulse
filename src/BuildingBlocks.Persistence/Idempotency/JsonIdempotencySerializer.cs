using System.Text.Json;
using BuildingBlocks.Application.Abstractions.Idempotency;
using BuildingBlocks.Results;

namespace BuildingBlocks.Persistence.Idempotency;

public class JsonIdempotencySerializer : IIdempotencySerializer
{
	private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

	public string Serialize<T>(Result<T> result)
	{
		return JsonSerializer.Serialize(result, JsonSerializerOptions);
	}

	public Result<T> Deserialize<T>(string value)
	{
		return JsonSerializer.Deserialize<Result<T>>(value, JsonSerializerOptions)
			   ?? throw new InvalidOperationException("Failed to deserialize idempotency result.");
	}
}