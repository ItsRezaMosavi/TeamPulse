namespace BuildingBlocks.Domain.Rules;

public sealed class Clause
{
    private const string NoStatement = "";

    private Clause(bool isValid, string statement, IReadOnlyCollection<ClauseSource> sources)
    {
        IsValid = isValid;
        Statement = statement;
        Sources = sources;
    }

    public bool IsValid { get; }
    public bool IsInvalid => !IsValid;
    public string Statement { get; }
    public IReadOnlyCollection<ClauseSource> Sources { get; }


    public static Clause Valid() => new(true, NoStatement, []);

    public static Clause Invalid(string statement, params ClauseSource[] sources)
    {
        statement = statement.Trim();
        return string.IsNullOrWhiteSpace(statement)
            ? throw new ArgumentException("Invalid clause requires a statement", nameof(statement))
            : new Clause(false, statement, sources?.ToArray() ?? []);
    }
}