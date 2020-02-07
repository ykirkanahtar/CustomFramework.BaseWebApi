using System.Linq;
using CustomFramework.BaseWebApi.Contracts.ApiContracts;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CustomFramework.BaseWebApi.Utils.Utils
{
    public class ApiRequestAccessor : IApiRequestAccessor
    {
        private readonly IHttpContextAccessor _accessor;

        public ApiRequestAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public T GetApiRequest<T>()
        {
            var apiRequestJson = _accessor.HttpContext.User.Claims.FirstOrDefault(p => p.Type == typeof(IApiRequest).Name)?.Value;

            return string.IsNullOrEmpty(apiRequestJson)
                ? default(T)
                : JsonConvert.DeserializeObject<T>(apiRequestJson);
        }
    }
}