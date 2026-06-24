using BuildingBlocks.Specification.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Persistence.Specifications;

public sealed class EfSpecificationEvaluator : ISpecificationEvaluator
{
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