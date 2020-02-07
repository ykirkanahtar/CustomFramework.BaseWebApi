using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Resources;
using CustomFramework.BaseWebApi.Data.Contracts;
using CustomFramework.BaseWebApi.Data.Enums;
using CustomFramework.BaseWebApi.Data.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using CustomFramework.BaseWebApi.Contracts;

namespace CustomFramework.BaseWebApi.Data.Repositories
{
    public abstract class AbstractRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : BaseModelNonUser<TKey>
    {
        protected readonly DbContext DbContext;
        protected readonly DbSet<TEntity> DbSet;
        private bool _disposed;
        private readonly Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> _includes;

        protected AbstractRepository(DbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<TEntity>();
        }
        protected AbstractRepository(DbContext dbContext, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<TEntity>();
            _includes = includes;
        }

        protected AbstractRepository(DbContext dbContext, bool eager)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<TEntity>();
        }

        #region IRepository members

        private IQueryable<TEntity> GetQueryByStatusSelector(TKey id, StatusSelector statusSelector)
        {
            switch (statusSelector)
            {
                case StatusSelector.OnlyActives:
                    return from p in DbContext.Set<TEntity>()
                           where p.Id.Equals(id) && p.Status == Status.Active
                           select p;
                case StatusSelector.ActivesAndPassives:
                    return from p in DbContext.Set<TEntity>()
                           where p.Id.Equals(id) && (p.Status == Status.Active || p.Status == Status.Passive)
                           select p;
                case StatusSelector.ActivesAndDeleted:
                    return from p in DbContext.Set<TEntity>()
                           where p.Id.Equals(id) && (p.Status == Status.Active || p.Status == Status.Deleted)
                           select p;
                case StatusSelector.PassivesAndDeleted:
                    return from p in DbContext.Set<TEntity>()
                           where p.Id.Equals(id) && (p.Status == Status.Passive || p.Status == Status.Deleted)
                           select p;
                case StatusSelector.OnlyPassives:
                    return from p in DbContext.Set<TEntity>()
                           where p.Id.Equals(id) && (p.Status == Status.Passive)
                           select p;
                case StatusSelector.OnlyDeleted:
                    return from p in DbContext.Set<TEntity>()
                           where p.Id.Equals(id) && (p.Status == Status.Deleted)
                           select p;
                case StatusSelector.All:
                    return from p in DbContext.Set<TEntity>()
                           where p.Id.Equals(id) && (p.Status == Status.Active || p.Status == Status.Passive || p.Status == Status.Deleted)
                           select p;
                default:
                    return from p in DbContext.Set<TEntity>()
                           where p.Id.Equals(id) && p.Status == Status.Active
                           select p;
            }
        }

        public async virtual Task<TEntity> GetByIdAsync(TKey id, StatusSelector statusSelector = StatusSelector.OnlyActives)
        {
            var query = GetQueryByStatusSelector(id, statusSelector);

            //var s = query.ToSql();

            if (_includes != null)
            {
                query = _includes(query);
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual TEntity GetById(TKey id, StatusSelector statusSelector = StatusSelector.OnlyActives)
        {
            var query = GetQueryByStatusSelector(id, statusSelector);

            if (_includes != null)
            {
                query = _includes(query);
            }

            return query.FirstOrDefault();
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, StatusSelector statusSelector = StatusSelector.OnlyActives)
        {
            return DbSet.Where(PredicateBuild(predicate, statusSelector));
        }

        //OrderBy kullanımı örneği : GetAll(orderBy: q => q.OrderByDescending(s => s.CreateDateTime), take: 10)
        public virtual IQueryable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? take = null, StatusSelector statusSelector = StatusSelector.OnlyActives
        )
        {
            IQueryable<TEntity> query = DbSet;

            query = query.Where(predicate != null ? PredicateBuild(predicate, statusSelector) : PredicateBuild(statusSelector));

            if (_includes != null)
            {
                query = _includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (take != null)
            {
                query = query.Take((int)take);
            }

            return query;
        }

        public virtual async Task<ICustomQueryable<TEntity>> GetAllWithPagingAsync(
            IPaging paging
            , Expression<Func<TEntity, bool>> predicate = null
            , Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
            , Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null
            , StatusSelector statusSelector = StatusSelector.OnlyActives
        )
        {
            IQueryable<TEntity> query = DbSet;

            query = query.Where(predicate != null ? PredicateBuild(predicate, statusSelector) : PredicateBuild(statusSelector));

            var rowCount = await query.CountAsync();

            if (_includes != null)
            {
                query = _includes(query);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            query = query.Skip(Math.Abs(paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize);

            var pageCount = (rowCount + paging.PageSize - 1) / paging.PageSize;

            return new CustomQueryable<TEntity>
            {
                Result = query,
                TotalCount = rowCount,
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                PageCount = pageCount
            };
        }

        public virtual ICustomQueryable<TEntity> GetAllWithPaging(
            IPaging paging, Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, StatusSelector statusSelector = StatusSelector.OnlyActives
        )
        {
            IQueryable<TEntity> query = DbSet;

            query = query.Where(predicate != null ? PredicateBuild(predicate, statusSelector) : PredicateBuild(statusSelector));

            var rowCount = query.Count();

            if (_includes != null)
            {
                query = _includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            query = query.Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize);

            var pageCount = (rowCount + paging.PageSize - 1) / paging.PageSize;

            return new CustomQueryable<TEntity>
            {
                Result = query,
                TotalCount = rowCount,
                PageIndex = paging.PageIndex,
                PageSize = paging.PageSize,
                PageCount = pageCount
            };
        }

        #endregion

        #region IDisposable members
        ~AbstractRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                DbContext.Dispose();
            }

            _disposed = true;
        }

        private static Expression<Func<TEntity, bool>> PredicateBuild(Expression<Func<TEntity, bool>> predicate, StatusSelector statusSelector = StatusSelector.OnlyActives)
        {
            var innerPredicate = PredicateBuilder.New<TEntity>();
            switch (statusSelector)
            {
                case StatusSelector.OnlyActives:
                    innerPredicate = innerPredicate.And(p => (int)p.Status == (int)Status.Active);
                    break;
                case StatusSelector.ActivesAndPassives:
                    innerPredicate = innerPredicate.And(p => (int)p.Status == (int)Status.Active).Or(p => (int)p.Status == (int)Status.Passive);
                    break;
                case StatusSelector.ActivesAndDeleted:
                    innerPredicate = innerPredicate.Or(p => (int)p.Status == (int)Status.Active).Or(p => (int)p.Status == (int)Status.Deleted);
                    break;
                case StatusSelector.PassivesAndDeleted:
                    innerPredicate = innerPredicate.Or(p => (int)p.Status == (int)Status.Passive).Or(p => (int)p.Status == (int)Status.Deleted);
                    break;
                case StatusSelector.OnlyPassives:
                    innerPredicate = innerPredicate.And(p => (int)p.Status == (int)Status.Passive);
                    break;
                case StatusSelector.OnlyDeleted:
                    innerPredicate = innerPredicate.And(p => (int)p.Status == (int)Status.Deleted);
                    break;
                case StatusSelector.All:
                    innerPredicate = innerPredicate.Or(p => (int)p.Status == (int)Status.Active).Or(p => (int)p.Status == (int)Status.Passive).Or(p => (int)p.Status == (int)Status.Deleted);
                    break;
                default:
                    innerPredicate = innerPredicate.And(p => (int)p.Status == (int)Status.Active);
                    break;
            }
            return predicate.And(innerPredicate);
        }

        private static Expression<Func<TEntity, bool>> PredicateBuild(StatusSelector statusSelector = StatusSelector.OnlyActives)
        {
            var predicate = PredicateBuilder.New<TEntity>(true);
            return PredicateBuild(predicate, statusSelector);
        }

        #endregion
    }
}
