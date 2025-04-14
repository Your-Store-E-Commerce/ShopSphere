using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Data.Specification
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, IBaseSpecification<T> spec)
        {
            var query = inputQuery;

            if (spec.Criteria is not null)
                query = inputQuery.Where(spec.Criteria);

            if (spec.OrderBy is not null)
                query = inputQuery.OrderBy(spec.OrderBy);

            else if (spec.OrderByDesc is not null)
                query = inputQuery.OrderByDescending(spec.OrderByDesc);

            if (spec.IsPaginationEnable)
                query = inputQuery.Skip(spec.Skip).Take(spec.Take);

            query = spec.Includes.Aggregate(inputQuery, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));


            return query;

        }
    }
}
