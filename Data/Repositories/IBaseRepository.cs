using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Data.Contracts;
using CustomFramework.BaseWebApi.Data.Enums;
using CustomFramework.BaseWebApi.Data.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace CustomFramework.BaseWebApi.Data.Repositories
{
    public interface IBaseRepository<TEntity, in TKey> : IDisposable
    where TEntity : BaseModelNonUser<TKey>
    {
        Task<TEntity> GetByIdAsync(TKey id, StatusSelector statusSelector = StatusSelector.OnlyActives);

        TEntity GetById(TKey id, StatusSelector statusSelector = StatusSelector.OnlyActives);

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, StatusSelector statusSelector = StatusSelector.OnlyActives);

        IQueryable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? take = null, StatusSelector statusSelector = StatusSelector.OnlyActives
        );

        Task<ICustomQueryable<TEntity>> GetAllWithPagingAsync(
            IPaging paging
            , Expression<Func<TEntity, bool>> predicate = null
            , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
            , Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
            , StatusSelector statusSelector = StatusSelector.OnlyActives
        );

        ICustomQueryable<TEntity> GetAllWithPaging(
            IPaging paging, Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, StatusSelector statusSelector = StatusSelector.OnlyActives
        );
    }
}