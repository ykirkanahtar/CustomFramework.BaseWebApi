using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Data.Models;
using CustomFramework.BaseWebApi.Data.Repositories;

namespace CustomFramework.BaseWebApi.Data
{
    public interface IUnitOfWork : IDisposable
    {
        BaseRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseModel<TKey>;

        BaseRepositoryNonUser<TEntity, TKey> GetRepositoryNonUser<TEntity, TKey>() where TEntity : BaseModelNonUser<TKey>;

        List<EntityChange> GetChanges<TEntity>();
        
        int SaveChanges();

        Task<int> SaveChangesAsync();

        int ExecuteSqlCommand(string sql, params object[] parameters);

        IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class;
    }
}