using BuildingBlocks.Domain.Rules;

namespace BuildingBlocks.Domain.Exceptions;

public sealed class DomainRulesException : Exception
{
    public DomainRulesException(IEnumerable<Clause> clauses)
        : base(BuildMessage(clauses, out var invalidClauses))
    {
        Clauses = invalidClauses.AsReadOnly();
    }

    public IReadOnlyCollection<Clause> Clauses { get; }

    private static string BuildMessage(IEnumerable<Clause> clauses, out Clause[] invalidClauses)
    {
        ArgumentNullException.ThrowIfNull(clauses);

        var array = clauses.ToArray();

        if (array.Length == 0)
            throw new ArgumentException($"{nameof(clauses)} cannot be empty.", nameof(clauses));

        foreach (var clause in array)
            ArgumentNullException.ThrowIfNull(clause);

        invalidClauses = array.Where(x => x.IsInvalid).ToArray();

        if (invalidClauses.Length == 0)
            throw new ArgumentException("No broken domain rules were provided.", nameof(clauses));

        return string.Join("; ", invalidClauses.Select(x => x.Statement));
    }
}