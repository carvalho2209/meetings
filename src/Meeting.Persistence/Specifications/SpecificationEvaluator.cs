using Meeting.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Meeting.Persistence.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> inputQueryable,
        Specification<TEntity> specification)
        where TEntity : Entity
    {
        IQueryable<TEntity> queryable = inputQueryable;

        if (specification.Criteria is not null)
        {
            queryable = queryable.Where(specification.Criteria);
        }

        queryable = specification.IncludeExpressions.Aggregate(
            queryable,
            (current, includeExpression) =>
                current.Include(includeExpression));

        if (specification.OrderByExpressions is not null)
        {
            queryable = queryable.OrderBy(specification.OrderByExpressions);
        }
        else if (specification.OrderByDescendingExpressions is not null)
        {
            queryable = queryable.OrderByDescending(
                specification.OrderByDescendingExpressions);
        }

        if (specification.IsSplitQuery)
        {
            queryable = queryable.AsSplitQuery();
        }

        return queryable;
    }
}