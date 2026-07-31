using BuildingBlocks.Specification.Contracts;

namespace BuildingBlocks.Persistence.Extensions;

public static class SpecificationExtensions
{
	public static IQueryable<T> WithSpecification<T>(this IQueryable<T> query,
													 IQuerySpecification<T> specification,
													 ISpecificationEvaluator evaluator) where T : class
	{
		return evaluator.GetQuery(query, specification);
	}
}