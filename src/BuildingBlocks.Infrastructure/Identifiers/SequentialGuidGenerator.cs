using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Application.Context;

namespace BuildingBlocks.Infrastructure.Identifiers;

public sealed class SequentialGuidGenerator(IDateTimeProvider dateTimeProvider) : IGuidGenerator
{
    public Guid Generate()
    {
        var randomBytes = Guid.NewGuid().ToByteArray();
        var timestampBytes = BitConverter.GetBytes(dateTimeProvider.UtcNow.Ticks);

        if (BitConverter.IsLittleEndian)
            Array.Reverse(timestampBytes);

        randomBytes[10] = timestampBytes[2];
        randomBytes[11] = timestampBytes[3];
        randomBytes[12] = timestampBytes[4];
        randomBytes[13] = timestampBytes[5];
        randomBytes[14] = timestampBytes[6];
        randomBytes[15] = timestampBytes[7];

        return new Guid(randomBytes);
    }
}