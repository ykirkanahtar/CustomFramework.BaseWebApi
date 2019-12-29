using CustomFramework.BaseWebApi.LogProvider.Data;
using CustomFramework.BaseWebApi.LogProvider.Models;
using System.Threading.Tasks;

namespace CustomFramework.BaseWebApi.LogProvider.Business
{
    public class LogManager : ILogManager
    {
        private readonly IUnitOfWorkLog _uow;

        public LogManager(IUnitOfWorkLog uow)
        {
            _uow = uow;
        }

        public async Task<Log> CreateAsync(Log log)
        {
            _uow.Logs.Add(log);
            await _uow.SaveChangesAsync();
            return log;
        }
    }
}