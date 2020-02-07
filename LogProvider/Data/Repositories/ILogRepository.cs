using CustomFramework.BaseWebApi.Data.Repositories;
using CustomFramework.BaseWebApi.LogProvider.Models;

namespace CustomFramework.BaseWebApi.LogProvider.Data.Repositories
{
    public interface ILogRepository : IRepositoryNonUser<Log, long>
    {

    }
}