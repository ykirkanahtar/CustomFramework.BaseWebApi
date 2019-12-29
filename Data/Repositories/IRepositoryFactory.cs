using CustomFramework.BaseWebApi.Data.Models;

namespace CustomFramework.BaseWebApi.Data.Repositories
{
    public interface IRepositoryFactory
    {
        BaseRepository<TEntity,TKey> GetRepository<TEntity, TKey>() where TEntity : BaseModel<TKey>;

        BaseRepositoryNonUser<TEntity, TKey> GetRepositoryNonUser<TEntity, TKey>() where TEntity : BaseModelNonUser<TKey>;
    }
}