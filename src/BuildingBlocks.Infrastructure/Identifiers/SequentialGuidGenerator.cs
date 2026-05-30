using BuildingBlocks.Application.Abstractions;

namespace BuildingBlocks.Infrastructure;

public sealed class SequentialGuidGenerator : IGuidGenerator
{
    public Guid Generate()
    {
        var randomBytes = Guid.NewGuid().ToByteArray();
        var timestampBytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
        
        if(BitConverter.IsLittleEndian)
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