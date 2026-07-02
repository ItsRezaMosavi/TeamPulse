using BuildingBlocks.Specification.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Specifications;

/// <summary>
/// Evaluates query specifications against Entity Framework Core queryables.
/// </summary>
/// <remarks>
/// This class applies specification criteria, includes, ordering, and pagination
/// to EF Core queries. It translates the abstract specification pattern into
/// concrete LINQ operations that EF Core can execute against the database.
/// 
/// Usage:
/// <code>
/// var evaluator = new EfSpecificationEvaluator();
/// var query = evaluator.GetQuery(context.Entities, specification);
/// </code>
/// </remarks>
public sealed class EfSpecificationEvaluator : ISpecificationEvaluator
{
    /// <summary>
    /// Applies a specification to an IQueryable to build the final query.
    /// </summary>
    /// <typeparam name="T">The type of entity being queried.</typeparam>
    /// <param name="query">The base IQueryable to apply the specification to.</param>
    /// <param name="specification">The specification containing criteria, includes, ordering, and pagination settings.</param>
    /// <returns>An IQueryable with all specification configurations applied.</returns>
    /// <remarks>
    /// The method applies specification components in this order:
    /// <list type="number">
    /// <item><description>WHERE clause from Criteria if present</description></item>
    /// <item><description>INCLUDE statements for related data</description></item>
    /// <item><description>ORDER BY or ORDER BY DESCENDING based on IsAscending</description></item>
    /// <item><description>SKIP and TAKE for pagination if enabled</description></item>
    /// </list>
    /// </remarks>
    public IQueryable<T> GetQuery<T>(IQueryable<T> query, IQuerySpecification<T> specification) where T : class
    {
        if (specification.Criteria is not null)
            query = query.Where(specification.Criteria);

        
        foreach (var include in specification.Includes)
        {
            query = query.Include(include);
        }
        
        if (specification.OrderBy is not null)
        {
            query = specification.IsAscending ?
                query.OrderBy(specification.OrderBy) :
                query.OrderByDescending(specification.OrderBy);
        }

        if (specification.IsPagingEnabled)
            query = query.Skip(specification.Skip).Take(specification.Take);

        return query;
    }
}