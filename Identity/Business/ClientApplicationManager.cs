using System.Reflection;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoMapper;
using CustomFramework.BaseWebApi.Contracts.Requests;
using CustomFramework.BaseWebApi.Data.Utils;
using CustomFramework.BaseWebApi.Identity.Constants;
using CustomFramework.BaseWebApi.Identity.Data;
using CustomFramework.BaseWebApi.Identity.Models;
using CustomFramework.BaseWebApi.Utils.Business;
using CustomFramework.BaseWebApi.Utils.Enums;
using CustomFramework.BaseWebApi.Utils.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomFramework.BaseWebApi.Identity.Business
{
    public class ClientApplicationManager : BaseBusinessManager, IClientApplicationManager
    {
        private readonly IUnitOfWorkIdentity _uow;

        public ClientApplicationManager(IUnitOfWorkIdentity uow, ILogger<ClientApplicationManager> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(logger, mapper, httpContextAccessor)
        {
            _uow = uow;
        }

        public Task<ClientApplication> CreateAsync(ClientApplicationRequest request)
        {
            return CommonOperationAsync(async() =>
            {
                var result = Mapper.Map<ClientApplication>(request);

                var tempResult = await _uow.ClientApplications.GetByNameAsync(result.ClientApplicationName);
                tempResult.CheckUniqueValue(IdentityStringMessages.ClientApplicationName);

                tempResult = await _uow.ClientApplications.GetByCodeAsync(result.ClientApplicationCode);
                tempResult.CheckUniqueValue(IdentityStringMessages.ClientApplicationCode);

                var salt = HashString.GetSalt();
                var hashPassword = HashString.Hash(result.ClientApplicationPassword, salt,
                    IdentityUtilConstants.IterationCountForHashing);

                result.ClientApplicationPassword = hashPassword;
                result.SecurityStamp = salt;

                _uow.ClientApplications.Add(result, GetUserId());

                await _uow.SaveChangesAsync();
                return result;
            }, new BusinessBaseRequest { MethodBase = MethodBase.GetCurrentMethod() });
        }

        public Task<ClientApplication> UpdateAsync(int id, ClientApplicationUpdateRequest request)
        {
            return CommonOperationAsync(async() =>
            {
                var result = await GetByIdAsync(id);
                Mapper.Map(request, result);

                var tempResult = await _uow.ClientApplications.GetByNameAsync(result.ClientApplicationName);
                tempResult.CheckUniqueValueForUpdate(id, IdentityStringMessages.ClientApplicationName);

                tempResult = await _uow.ClientApplications.GetByCodeAsync(result.ClientApplicationCode);
                tempResult.CheckUniqueValueForUpdate(id, IdentityStringMessages.ClientApplicationCode);

                _uow.ClientApplications.Update(result, GetUserId());

                await _uow.SaveChangesAsync();
                return result;

            }, new BusinessBaseRequest { MethodBase = MethodBase.GetCurrentMethod() });
        }

        public Task<ClientApplication> UpdateClientApplicationPasswordAsync(int id, string clientApplicationPassword)
        {
            return CommonOperationAsync(async() =>
            {
                var result = await GetByIdAsync(id);

                var salt = HashString.GetSalt();
                var hashPassword = HashString.Hash(clientApplicationPassword, salt,
                    IdentityUtilConstants.IterationCountForHashing);

                result.SecurityStamp = salt;
                result.ClientApplicationPassword = hashPassword;

                _uow.ClientApplications.Update(result, GetUserId());

                await _uow.SaveChangesAsync();
                return result;
            }, new BusinessBaseRequest { MethodBase = MethodBase.GetCurrentMethod() });
        }

        public Task DeleteAsync(int id)
        {
            return CommonOperationAsync(async() =>
            {
                var result = await GetByIdAsync(id);

                _uow.ClientApplications.Delete(result, GetUserId());

                await _uow.SaveChangesAsync();
            }, new BusinessBaseRequest { MethodBase = MethodBase.GetCurrentMethod() });
        }

        public Task<ClientApplication> GetByIdAsync(int id)
        {
            return CommonOperationAsync(async() => await _uow.ClientApplications.GetByIdAsync(id), new BusinessBaseRequest { MethodBase = MethodBase.GetCurrentMethod() },
                BusinessUtilMethod.CheckRecordIsExist, GetType().Name);
        }

        public Task<ClientApplication> GetByClientApplicationCodeAsync(string code)
        {
            return CommonOperationAsync(async() => await _uow.ClientApplications.GetByCodeAsync(code), new BusinessBaseRequest { MethodBase = MethodBase.GetCurrentMethod() },
                BusinessUtilMethod.CheckRecordIsExist, GetType().Name);
        }

        public Task<ClientApplication> LoginAsync(string code, string password)
        {
            return CommonOperationAsync(async() =>
            {
                var clientApplication = await _uow.ClientApplications.GetByCodeAsync(code);
                if (clientApplication == null)
                    throw new AuthenticationException();

                password = HashString.Hash(password, clientApplication.SecurityStamp,
                    IdentityUtilConstants.IterationCountForHashing);

                var client = await _uow.ClientApplications.GetByCodeAndPasswordAsync(code, password);
                if (client == null) throw new AuthenticationException();

                return client;
            }, new BusinessBaseRequest { MethodBase = MethodBase.GetCurrentMethod() });
        }
    }
}