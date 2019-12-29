using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Identity.Models;
using System;
using CustomFramework.BaseWebApi.Data.Contracts;

namespace CustomFramework.BaseWebApi.Identity.Data.Repositories
{
    public interface ICustomUserRepository<TUser>
        where TUser : CustomUser
    {
        Task<ICustomList<TUser>> GetOnlineUsers(int sessionMinutes, int pageIndex, int pageSize, DateTime? DateTimeNowValue = null);
    }
}