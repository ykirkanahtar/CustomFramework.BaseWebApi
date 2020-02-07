using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using CustomFramework.BaseWebApi.Data.Repositories;
using CustomFramework.BaseWebApi.Data.Models;

//https://github.com/Arch/UnitOfWork/blob/master/src/Microsoft.EntityFrameworkCore.UnitOfWork/UnitOfWork.cs
namespace CustomFramework.BaseWebApi.Data
{
    public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext> where TContext : DbContext
    {
        private bool _disposed;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(TContext context)
        {
            DbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TContext DbContext { get; }

        public BaseRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseModel<TKey>
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new BaseRepository<TEntity, TKey>(DbContext);
            }

            return (BaseRepository<TEntity, TKey>)_repositories[type];
        }

        public BaseRepositoryNonUser<TEntity, TKey> GetRepositoryNonUser<TEntity, TKey>() where TEntity : BaseModelNonUser<TKey>
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new BaseRepositoryNonUser<TEntity, TKey>(DbContext);
            }

            return (BaseRepositoryNonUser<TEntity, TKey>)_repositories[type];
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters) => DbContext.Database.ExecuteSqlRaw(sql, parameters);

        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class => DbContext.Set<TEntity>().FromSqlRaw(sql, parameters);

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public List<EntityChange> GetChanges<TEntity>()
        {
            var entityChanges = new List<EntityChange>();

            var modifiedEntities = DbContext.ChangeTracker.Entries()
                        .Where(p => p.State == EntityState.Modified || p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified || p.State == EntityState.Detached).ToList();

            foreach (var change in modifiedEntities)
            {
                var entityName = change.Entity.GetType().Name;
                var entityIdObj = change.Property("Id").CurrentValue;
                long? entityId = null;

                if (entityIdObj != null) entityId = Convert.ToInt64(entityIdObj.ToString());

                foreach (var prop in change.Entity.GetType().GetTypeInfo().DeclaredProperties)
                {
                    if (!prop.GetGetMethod().IsVirtual)
                    {
                        object oldValueObj = null;
                        object newValueObj = null;

                        if (change.State == EntityState.Deleted || change.State == EntityState.Modified)
                        {
                            oldValueObj = change.GetDatabaseValues().GetValue<object>(prop.Name);
                        }

                        if (change.State == EntityState.Added || change.State == EntityState.Modified)
                        {
                            newValueObj = change.Property(prop.Name).CurrentValue;
                        }

                        var newValue = newValueObj == null ? string.Empty : newValueObj.ToString();
                        var oldValue = oldValueObj == null ? string.Empty : oldValueObj.ToString();

                        if (oldValue != newValue)
                        {
                            entityChanges.Add(new EntityChange
                            {
                                EntityName = entityName,
                                FieldName = prop.Name,
                                IdValue = entityId,
                                EntityState = change.State,
                                OldValue = oldValue,
                                NewValue = newValue
                            });
                        }
                    }
                }
            }

            return entityChanges;
        }



        public async Task<int> SaveChangesAsync()
        {
            OnBeforeSaving();
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync(params IUnitOfWork[] unitOfWorks)
        {
            // TransactionScope will be included in .NET Core v2.0
            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var count = 0;
                    foreach (var unitOfWork in unitOfWorks)
                    {
                        if (!(unitOfWork is UnitOfWork<DbContext> uow)) continue;
                        uow.DbContext.Database.UseTransaction(transaction.GetDbTransaction());
                        count += await uow.SaveChangesAsync();
                    }

                    count += await SaveChangesAsync();

                    transaction.Commit();

                    return count;
                }
                catch (Exception)
                {

                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void OnBeforeSaving()
        {
            var entries = DbContext?.ChangeTracker?.Entries();

            if (entries == null)
            {
                return;
            }

            foreach (var entry in entries)
            {
                // get all the properties and are of type string
                var propertyValues = entry.CurrentValues.Properties.Where(p => p.ClrType == typeof(string));

                foreach (var prop in propertyValues)
                {
                    // access the correct column by it's name and trim the value if it's not null
                    if (entry.CurrentValues[prop.Name] != null) entry.CurrentValues[prop.Name] = entry.CurrentValues[prop.Name].ToString().Trim();
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                // clear repositories
                _repositories?.Clear();

                // dispose the db context.
                DbContext.Dispose();
            }

            _disposed = true;
        }
    }

}