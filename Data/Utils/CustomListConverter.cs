using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CustomFramework.BaseWebApi.Data.Utils
{
    public static class CustomListConverter
    {
        public static async Task<ICustomList<T>> ToCustomListAsync<T>(this ICustomQueryable<T> query, Paging paging)
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

        public static async Task<ICustomList<T>> ToCustomListAsync<T>(this IQueryable<T> result, Paging paging) where T : class
        {
            var list = await result.ToListAsync();
            return new CustomList<T>
            {
                Result = list,
                TotalCount = list.Count,
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                PageCount = list.Count / paging.PageSize
            };
        }

        public static ICustomList<T> ToCustomList<T>(this IList<T> result, Paging paging) where T : class
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

        public async static Task<ICustomList<T>> GetCustomListFromQueryAsync<T>(this IQueryable<T> query, Paging paging, bool applyPagingToquery = true, int? count = null) where T : class
        {
            int rowCount = count == null ?  query.Count() : (int)count;
            if (applyPagingToquery) query = query.Skip(Math.Abs(paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize);

            var pageCount = (rowCount + paging.PageSize - 1) / paging.PageSize;

            var customQuery = new CustomQueryable<T>
            {
                Result = query,
                TotalCount = rowCount,
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                PageCount = pageCount
            };

            var result = await customQuery.ToCustomListAsync(paging);

            return result;
        }

    }
}