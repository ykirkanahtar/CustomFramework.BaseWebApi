using System;
using System.Collections.Generic;
using CustomFramework.BaseWebApi.Resources;
using CustomFramework.BaseWebApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using CustomFramework.BaseWebApi.Contracts;

namespace CustomFramework.BaseWebApi.Data.Utils
{
    public static class SeedDataUtil
    {
        public static void SeedTData<T, TKey>(ModelBuilder modelBuilder, IEnumerable<T> list, int userId) where T : BaseModel<TKey>
        {
            foreach (var item in list)
            {
                SetCommonFields<T, TKey>(item, userId);
                modelBuilder.Entity<T>()
                    .HasData(item);
            }
        }

        public static void SetCommonFields<T, TKey>(T entity, int userId) where T : BaseModel<TKey>
        {
            entity.CreateDateTime = DateTime.Now;
            entity.CreateUserId = userId;
            entity.Status = Status.Active;
        }

    }
}