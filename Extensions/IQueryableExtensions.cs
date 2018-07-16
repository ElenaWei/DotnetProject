using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyDotnetProject.Core.Models;
using MyDotnetProject.Models;

namespace MyDotnetProject.Extensions
{
    public static class IQueryableExtensions
    {
    public static IQueryable<T> ApplyOrdering<T> (this IQueryable<T> query, IQueryObject queryObj, Dictionary<string, Expression<Func<T, object>>> columnsMap) 
        {   
            // edge case: id not exist, or parameters are not assigned.
             if (String.IsNullOrWhiteSpace(queryObj.SortBy) || !columnsMap.ContainsKey(queryObj.SortBy))
                return query;

            if (queryObj.IsSortAsceding)
                return  query.OrderBy(columnsMap[queryObj.SortBy]);
            else
                return query.OrderByDescending(columnsMap[queryObj.SortBy]);
        }
    }
}