using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Identity.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using CustomFramework.BaseWebApi.Data.Contracts;
using CustomFramework.BaseWebApi.Resources;
using CustomFramework.BaseWebApi.Data.Utils;
using CustomFramework.BaseWebApi.Contracts;
using CustomFramework.BaseWebApi.Contracts.ApiContracts;

namespace CustomFramework.BaseWebApi.Identity.Data.Repositories
{
    public class CustomUserRepository<TUser> : ICustomUserRepository<TUser>
    where TUser : CustomUser
    {
        private readonly DbContext _dbContext;

        public CustomUserRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICustomList<TUser>> GetOnlineUsers(int sessionMinutes, int pageIndex, int pageSize, DateTime? DateTimeNowValue = null)
        {
            var query = (from u in _dbContext.Set<TUser>().AsNoTracking()
                         where u.Status == Status.Active
                         && SqlServerDbFunctionsExtensions.DateDiffMinute(EF.Functions, u.LastSuccessfullLogin, DateTimeNowValue ?? DateTime.Now) < sessionMinutes
                         && u.LastSuccessfullLogin > u.LastLogOutDate
                         select u);
            return await query.ToCustomListAsync(new Paging(pageIndex, pageSize));
        }
    }
}