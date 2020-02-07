using System;
using CustomFramework.BaseWebApi.Data.Models;

namespace CustomFramework.BaseWebApi.Data.Repositories
{
    public interface IRepository<TEntity, in TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : BaseModel<TKey>
    {
        void Add(TEntity entity, int userId, DateTime? logDateTime = null, int? ClientApplicationId = null);
        void Update(TEntity entity, int userId, DateTime? logDateTime = null, int? ClientApplicationId = null);
        void Delete(TEntity entity, int userId, DateTime? logDateTime = null, int? ClientApplicationId = null);
    }
}