using CustomFramework.BaseWebApi.Data.Models;
using System.Threading.Tasks;

namespace CustomFramework.BaseWebApi.Utils.Business
{
    public interface IBusinessManagerUpdate<TEntity, in TUpdateRequest, in TKey>
        where TEntity : BaseModel<TKey>
    {
        Task<TEntity> UpdateAsync(TKey id, TUpdateRequest request);
    }
}