namespace BuildingBlocks.Domain.Rules;

public readonly struct ClauseSource
{
    public ClauseSource(string name, object? value = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("name cannot be null or white spaces",nameof(name));
        Name = name;
        Value = value;
    }

    public string Name { get; }
    public object? Value { get; }

    public override string ToString() =>
        Value is null ? Name : $"{Name}: {Value}";

    public static implicit operator ClauseSource(string name)
    {
        return new ClauseSource(name);
    }

    public static implicit operator ClauseSource((string Name, object? Value) nameValue)
    {
        return new ClauseSource(nameValue.Name, nameValue.Value);
    }
}