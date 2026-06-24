namespace BuildingBlocks.Specification.Contracts;

public interface ISpecificationEvaluator
{
    IQueryable<T> GetQuery<T>(IQueryable<T> query, IQuerySpecification<T> specification) where T : class;
}