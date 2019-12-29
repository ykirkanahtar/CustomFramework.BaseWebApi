using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Identity.Models;
using CustomFramework.BaseWebApi.Data.Repositories;

namespace CustomFramework.BaseWebApi.Identity.Data.Repositories
{
    public interface IClientApplicationRepository : IRepository<ClientApplication, int>
    {
        Task<ClientApplication> GetByNameAsync(string name);
        Task<ClientApplication> GetByCodeAsync(string code);
        Task<ClientApplication> GetByCodeAndPasswordAsync(string code, string password);
        
    }
}