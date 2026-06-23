using System.Linq.Expressions;
using BuildingBlocks.Specification.Composition;
using BuildingBlocks.Specification.Contracts;

namespace BuildingBlocks.Specification.Base;

public abstract class Specification<T> : ISpecification<T>
{
    private Func<T, bool>? _compiled;

    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T entity)
    {
        _compiled ??= ToExpression().Compile();
        return _compiled(entity);
    }

    public static implicit operator Expression<Func<T, bool>>(Specification<T> spec) => spec.ToExpression();

    public Specification<T> And(Specification<T> other) => new AndSpecification<T>(this, other);
    public Specification<T> Or(Specification<T> other) => new OrSpecification<T>(this, other);
    public Specification<T> Not() => new NotSpecification<T>(this);
}