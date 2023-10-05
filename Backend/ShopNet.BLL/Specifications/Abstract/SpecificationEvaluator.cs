using Microsoft.EntityFrameworkCore;
using ShopNet.DAL.Entities;

namespace ShopNet.BLL.Specifications.Abstract
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec)
        {
            if (spec.Criteria is not null) inputQuery = inputQuery.Where(spec.Criteria);
            if (spec.OrderBy is not null) inputQuery = inputQuery.OrderBy(spec.OrderBy);
            if (spec.OrderByDescending is not null) inputQuery = inputQuery.OrderByDescending(spec.OrderByDescending);
            if (spec.IsPagingEnabled) inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);

            inputQuery = spec.Includes.Aggregate(inputQuery, (current, include) => current.Include(include));
            return inputQuery;
        }
    }
}