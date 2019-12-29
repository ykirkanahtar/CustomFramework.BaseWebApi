using AutoMapper;
using CustomFramework.BaseWebApi.Contracts.ApiContracts;
using CustomFramework.BaseWebApi.Utils.Utils;
using Microsoft.Extensions.Logging;

namespace CustomFramework.BaseWebApi.Utils.Business
{
    public abstract class BaseBusinessManagerWithApiRequest<TApiRequest> : BaseBusinessManager
            where TApiRequest : IApiRequest
    {
        private readonly IApiRequest _apiRequestAccessor;

        protected BaseBusinessManagerWithApiRequest(ILogger<BaseBusinessManager> logger, IMapper mapper, IApiRequestAccessor apiRequestAccessor)
            : base(logger, mapper)
        {
            _apiRequestAccessor = apiRequestAccessor.GetApiRequest<TApiRequest>();
        }

        protected int GetLoggedInUserId()
        {
            return _apiRequestAccessor.UserId;
        }

        protected int GetNullableLoggedInUserId(int returnValue)
        {
            if(_apiRequestAccessor == null) return returnValue;
            return _apiRequestAccessor.UserId;
        }
    }
}