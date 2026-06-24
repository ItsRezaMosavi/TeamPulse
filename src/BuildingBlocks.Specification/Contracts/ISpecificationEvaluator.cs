namespace BuildingBlocks.Specification.Contracts;

public interface ISpecificationEvaluator
{
    IQueryable<T> GetQuery<T>(IQueryable<T> query, IQuerySpecification<T> specification,
                                    CancellationToken cancellationToken = default) where T : class;
}