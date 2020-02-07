using CustomFramework.BaseWebApi.Data.Models;

namespace CustomFramework.BaseWebApi.Data.Repositories
{
    public interface IRepositoryNonUser<TEntity, in TKey> : IBaseRepository<TEntity, TKey>
        where TEntity : BaseModelNonUser<TKey>
    {
        void Add(TEntity entity, int? ClientApplicationId = null);
        void Update(TEntity entity, int? ClientApplicationId = null);
        void Delete(TEntity entity, int? ClientApplicationId = null);
    }
}