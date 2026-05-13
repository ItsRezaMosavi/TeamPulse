using BuildingBlocks.Validation.Exceptions;

namespace BuildingBlocks.Validation.Extensions;

public static class ClauseCollectionExtensions
{
    public static void Throw(this IEnumerable<Clause> clauses)
    {
        ArgumentNullException.ThrowIfNull(clauses);

        var array = clauses.ToArray();

        foreach (var clause in array)
            ArgumentNullException.ThrowIfNull(clause);

        if (array.Length > 0)
            throw new DomainRulesException(array);
    }
}