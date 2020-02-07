using CustomFramework.BaseWebApi.Data.Models;

namespace CustomFramework.BaseWebApi.Data.Repositories
{
    public interface IRepositoryNonUserFactory
    {
        IRepositoryNonUser<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseModelNonUser<TKey>;
    }
}