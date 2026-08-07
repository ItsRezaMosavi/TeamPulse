using System.Linq.Expressions;
using BuildingBlocks.Specification.Contracts;

namespace BuildingBlocks.Specification.Base;

public class QuerySpecification<T, TResult> : QuerySpecification<T>, IQuerySpecification<T, TResult>
{
	public Expression<Func<T, TResult>> Selector { get; protected set; } = null!;

	public void ApplySelector(Expression<Func<T, TResult>> selector)
	{
		Selector = selector;
	}
}