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

	public static IQueryable<TResult> WithSpecification<T, TResult>(this IQueryable<T> query,
																	IQuerySpecification<T, TResult> specification,
																	ISpecificationEvaluator evaluator) where T : class
	{
		return evaluator.GetQuery(query, specification);
	}
}