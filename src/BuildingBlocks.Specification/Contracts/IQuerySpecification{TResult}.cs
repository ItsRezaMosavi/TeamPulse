using System.Linq.Expressions;

namespace BuildingBlocks.Specification.Contracts;

public interface IQuerySpecification<T, TResult> : IQuerySpecification<T>
{
	Expression<Func<T, TResult>> Selector { get; }
}