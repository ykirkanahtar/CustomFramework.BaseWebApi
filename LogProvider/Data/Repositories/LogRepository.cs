using CustomFramework.BaseWebApi.Data.Repositories;
using CustomFramework.BaseWebApi.LogProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomFramework.BaseWebApi.LogProvider.Data.Repositories
{
    public class LogRepository : BaseRepositoryNonUser<Log, long>, ILogRepository
    {
        public LogRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}