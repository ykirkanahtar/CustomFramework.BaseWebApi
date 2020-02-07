using System.Threading.Tasks;
using CustomFramework.BaseWebApi.LogProvider.Models;

namespace CustomFramework.BaseWebApi.LogProvider.Business
{
    public interface ILogManager
    {
        Task<Log> CreateAsync(Log log);
    }
}