using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Data.Models;

namespace CustomFramework.BaseWebApi.Utils.Business
{
    public interface IBusinessManager<TEntity, in TCreateRequest, in TKey> where TEntity : BaseModel<TKey>
    {
        Task<TEntity> CreateAsync(TCreateRequest request);
        Task DeleteAsync(TKey id);
        Task<TEntity> GetByIdAsync(TKey id);
    }

    public interface IBusinessManager<TEntity, in TKey> where TEntity : BaseModel<TKey>
    {
        Task DeleteAsync(TKey id);
        Task<TEntity> GetByIdAsync(TKey id);
    }
}