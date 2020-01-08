using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Contracts.ApiContracts;
using CustomFramework.BaseWebApi.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CustomFramework.BaseWebApi.Data.Utils
{
    public static class CustomListConverter
    {
        public static async Task<ICustomList<T>> ToCustomListAsync<T>(this ICustomQueryable<T> query)
        where T : class
        {
            var result = await query.Result.ToListAsync();

            return new CustomList<T>
            {
                Result = result,
                TotalCount = query.TotalCount,
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                PageCount = query.PageCount
            };
        }

        public static async Task<ICustomList<T>> ToCustomListAsync<T>(this IQueryable<T> query, Paging paging, string orderBy = null) where T : class
        {
            int rowCount = query.Count();

            if(!String.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy(orderBy);
            }

            query = query.Skip(Math.Abs(paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize);

            var pageCount = (rowCount + paging.PageSize - 1) / paging.PageSize;

            var list = await query.ToListAsync();

            return new CustomList<T>
            {
                Result = list,
                TotalCount = rowCount,
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                PageCount = rowCount / paging.PageSize
            };
        }

        public static ICustomList<T> ToCustomList<T>(this List<T> result, Paging paging) where T : class
        {
            return new CustomList<T>
            {
                Result = result,
                TotalCount = result.Count,
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                PageCount = result.Count / paging.PageSize
            };
        }
    }
}