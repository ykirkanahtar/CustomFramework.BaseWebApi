using Microsoft.EntityFrameworkCore;
using System;
using CustomFramework.BaseWebApi.Data.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore.Query;
using CustomFramework.BaseWebApi.Contracts;

namespace CustomFramework.BaseWebApi.Data.Repositories
{
    public class BaseRepository<TEntity, TKey> : AbstractRepository<TEntity, TKey>, IRepository<TEntity, TKey>
        where TEntity : BaseModel<TKey>

    {
        public BaseRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public BaseRepository(DbContext dbContext, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes) : base(dbContext, includes)
        {

        }

        public virtual void Add(TEntity entity, int userId, DateTime? logDateTime = null, int? ClientApplicationId = null)
        {
            entity.CreateDateTime = logDateTime != null ? (DateTime)logDateTime : DateTime.UtcNow;
            entity.CreateUserId = userId;
            entity.CreateClientApplicationId = ClientApplicationId;
            entity.Status = Status.Active;

            DbSet.Add(entity);

            foreach (var entry in DbContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["CreateUserId"] = userId;
                        entry.CurrentValues["CreateDateTime"] = logDateTime != null ? (DateTime)logDateTime : DateTime.UtcNow;
                        entry.CurrentValues["CreateClientApplicationId"] = ClientApplicationId;
                        entry.CurrentValues["Status"] = Status.Active;
                        break;
                }
            }
        }

        public virtual void Update(TEntity entity, int userId, DateTime? logDateTime = null, int? ClientApplicationId = null)
        {
            entity.UpdateDateTime = logDateTime != null ? (DateTime)logDateTime : DateTime.UtcNow;
            entity.UpdateUserId = userId;
            entity.UpdateClientApplicationId = ClientApplicationId;

            foreach (var entry in DbContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["CreateUserId"] = userId;
                        entry.CurrentValues["CreateDateTime"] = logDateTime != null ? logDateTime : DateTime.UtcNow;
                        entry.CurrentValues["CreateClientApplicationId"] = ClientApplicationId;
                        entry.CurrentValues["Status"] = Status.Active;
                        break;
                    case EntityState.Deleted:
                        entry.CurrentValues["DeleteUserId"] = userId;
                        entry.CurrentValues["DeleteDateTime"] = logDateTime != null ? logDateTime : DateTime.UtcNow;
                        entry.CurrentValues["DeleteClientApplicationId"] = ClientApplicationId;
                        entry.CurrentValues["Status"] = Status.Deleted;
                        entry.State = EntityState.Modified;
                        break;
                    case EntityState.Detached:
                        entry.CurrentValues["DeleteUserId"] = userId;
                        entry.CurrentValues["DeleteDateTime"] = logDateTime != null ? logDateTime : DateTime.UtcNow;
                        entry.CurrentValues["DeleteClientApplicationId"] = ClientApplicationId;
                        entry.CurrentValues["Status"] = Status.Deleted;
                        entry.State = EntityState.Modified;
                        break;
                }
            }

            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity, int userId, DateTime? logDateTime = null, int? ClientApplicationId = null)
        {
            entity.DeleteDateTime = logDateTime != null ? (DateTime)logDateTime : DateTime.UtcNow;
            entity.DeleteUserId = userId;
            entity.DeleteClientApplicationId = ClientApplicationId;
            entity.Status = Status.Deleted;

            foreach (var entry in DbContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.CurrentValues["DeleteUserId"] = userId;
                        entry.CurrentValues["DeleteDateTime"] = logDateTime != null ? (DateTime)logDateTime : DateTime.UtcNow;
                        entry.CurrentValues["Status"] = Status.Deleted;
                        entry.CurrentValues["DeleteClientApplicationId"] = ClientApplicationId;
                        entry.State = EntityState.Modified;
                        break;
                }
            }

            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Deleted;
        }


        //Güncelleme işleminde çalışmadığı için kullanılmıyor.
        private void SyncChildClasses(TEntity entity, int userId, bool deleted, DateTime? logDateTime = null, int? ClientApplicationId = null)
        {
            foreach (var entry in DbContext.ChangeTracker.Entries<TEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Status = Status.Active;
                        entry.Entity.CreateDateTime = logDateTime != null ? (DateTime)logDateTime : DateTime.UtcNow;
                        entry.Entity.CreateUserId = userId;
                        entry.Entity.CreateClientApplicationId = ClientApplicationId;
                        break;

                    case EntityState.Modified:
                        if (deleted)
                        {
                            entry.Entity.Status = Status.Deleted;
                            entry.Entity.DeleteDateTime = logDateTime != null ? (DateTime)logDateTime : DateTime.UtcNow;
                            entry.Entity.DeleteUserId = userId;
                            entry.Entity.DeleteClientApplicationId = ClientApplicationId;
                            entry.State = EntityState.Modified;
                        }
                        else
                        {
                            entry.Entity.UpdateDateTime = logDateTime != null ? (DateTime)logDateTime : DateTime.UtcNow;
                            entry.Entity.UpdateUserId = userId;
                            entry.Entity.UpdateClientApplicationId = ClientApplicationId;
                        }
                        break;
                }
            }
        }

    }
}