using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Contracts.Requests;
using CustomFramework.BaseWebApi.Identity.Models;
using CustomFramework.BaseWebApi.Utils.Business;

namespace CustomFramework.BaseWebApi.Identity.Business
{
    public interface IClientApplicationManager : IBusinessManager<ClientApplication, ClientApplicationRequest, int>
                                                , IBusinessManagerUpdate<ClientApplication, ClientApplicationUpdateRequest, int>
    {
        Task<ClientApplication> UpdateClientApplicationPasswordAsync(int id, string clientApplicationPassword);

        Task<ClientApplication> GetByClientApplicationCodeAsync(string code);

        Task<ClientApplication> LoginAsync(string code, string password);
    }

}