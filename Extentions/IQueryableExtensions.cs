using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Vega.Extentions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, IQueryObject vehicleQuery, Dictionary<string, Expression<Func<T, object>>> columnsMap)
        {
            if (String.IsNullOrWhiteSpace(vehicleQuery.SortBy) || !columnsMap.ContainsKey(vehicleQuery.SortBy))
                return query;


            if (vehicleQuery.IsSortAscending)
                return query.OrderBy(columnsMap[vehicleQuery.SortBy]);

            else
                return query.OrderByDescending(columnsMap[vehicleQuery.SortBy]);

        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, IQueryObject vehicleQuery)
        {
            if (vehicleQuery.Page <= 0)
                vehicleQuery.Page = 1;

            if (vehicleQuery.PageSize <= 0)
                vehicleQuery.PageSize = 10;

            return query.Skip((vehicleQuery.Page - 1) * vehicleQuery.PageSize).Take(vehicleQuery.PageSize);
        }
    }



}
