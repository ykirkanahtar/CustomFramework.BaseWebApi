using Microsoft.EntityFrameworkCore;
using System;
using CustomFramework.BaseWebApi.Data.Models;
using CustomFramework.BaseWebApi.Contracts;

namespace CustomFramework.BaseWebApi.Data.Repositories
{
    public class BaseRepositoryNonUser<TEntity, TKey> : AbstractRepository<TEntity, TKey>, IRepositoryNonUser<TEntity, TKey>
        where TEntity : BaseModelNonUser<TKey>

    {
        public BaseRepositoryNonUser(DbContext dbContext) : base(dbContext)
        {

        }

        public virtual void Add(TEntity entity, int? ClientApplicationId = null)
        {
            entity.CreateDateTime = DateTime.Now;
            entity.CreateClientApplicationId = ClientApplicationId;
            entity.Status = Status.Active;
            DbSet.Add(entity);
        }

        public virtual void Update(TEntity entity, int? ClientApplicationId = null)
        {
            entity.UpdateDateTime = DateTime.Now;
            entity.UpdateClientApplicationId = ClientApplicationId;

            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity, int? ClientApplicationId = null)
        {
            entity.DeleteDateTime = DateTime.Now;
            entity.DeleteClientApplicationId = ClientApplicationId;
            entity.Status = Status.Deleted;

            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}