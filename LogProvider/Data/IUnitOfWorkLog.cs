using CustomFramework.BaseWebApi.Data;
using CustomFramework.BaseWebApi.LogProvider.Data.Repositories;

namespace CustomFramework.BaseWebApi.LogProvider.Data
{
    public interface IUnitOfWorkLog : IUnitOfWork
    {
        ILogRepository Logs { get; }
    }
}