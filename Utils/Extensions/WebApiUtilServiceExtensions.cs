using CustomFramework.BaseWebApi.Utils.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using CustomFramework.BaseWebApi.Contracts.ApiContracts;

namespace CustomFramework.BaseWebApi.Utils.Extensions
{
    public static class WebApiUtilServiceExtensions
    {
        public static IServiceCollection AddWebApiUtilServices(this IServiceCollection services)
        {
            services.AddTransient<IApiRequestAccessor, ApiRequestAccessor>();
            services.AddTransient<IApiRequest, ApiRequest>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddCors();

            return services;
        }
    }
}
