using System.Linq.Expressions;

namespace BuildingBlocks.Specification.Contracts;

public interface ISpecification<T>
{
    public Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity);
}