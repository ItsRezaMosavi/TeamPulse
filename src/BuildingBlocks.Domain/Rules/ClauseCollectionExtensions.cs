using BuildingBlocks.Domain.Exceptions;

namespace BuildingBlocks.Domain.Rules;

public static class ClauseCollectionExtensions
{
    public static void ThrowIfBroken(this IEnumerable<Clause> clauses)
    {
        ArgumentNullException.ThrowIfNull(clauses);

        var array = clauses.ToArray();

        foreach (var clause in array)
            ArgumentNullException.ThrowIfNull(clause);

        var invalidClauses = array.Where(c => c.IsInvalid).ToArray();
        if (invalidClauses.Length > 0)
            throw new DomainRulesException(invalidClauses);
    }
}