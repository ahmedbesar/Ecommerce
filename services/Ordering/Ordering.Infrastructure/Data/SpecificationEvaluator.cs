using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Specifications;

namespace Ordering.Infrastructure.Data
{
    public class SpecificationEvaluator<T> where T : EntityBase
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;

            // Apply criteria (where clause)
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            // Apply ordering
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            // Apply grouping
            if (spec.GroupBy != null)
            {
                query = query.GroupBy(spec.GroupBy).SelectMany(g => g);
            }

            // Apply paging
            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // Apply includes (eager loading)
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));


            return query;
        }
    }
}
