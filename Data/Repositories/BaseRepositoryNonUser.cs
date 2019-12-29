using Microsoft.EntityFrameworkCore;
using System;
using CustomFramework.BaseWebApi.Data.Models;
using CustomFramework.BaseWebApi.Resources;
using CustomFramework.BaseWebApi.Contracts;

namespace CustomFramework.BaseWebApi.Data.Repositories
{
    public class BaseRepositoryNonUser<TEntity, TKey> : AbstractRepository<TEntity, TKey>, IRepositoryNonUser<TEntity, TKey>
        where TEntity : BaseModelNonUser<TKey>

    {
        public BaseRepositoryNonUser(DbContext dbContext) : base(dbContext)
        {

        }

        public virtual void Add(TEntity entity)
        {
            entity.CreateDateTime = DateTime.Now;
            entity.Status = Status.Active;
            DbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            entity.UpdateDateTime = DateTime.Now;

            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            entity.DeleteDateTime = DateTime.Now;
            entity.Status = Status.Deleted;

            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}