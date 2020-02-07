using CustomFramework.BaseWebApi.LogProvider.Business;
using CustomFramework.BaseWebApi.LogProvider.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CustomFramework.BaseWebApi.LogProvider.Extensions
{
    public static class LogProviderServiceExtension
    {
        public static IServiceCollection AddLogModels(this IServiceCollection services)
        {
            /*********Repositories*********/
            services.AddTransient<ILogRepository, LogRepository>();
            /*********Repositories*********/

            /**********Managers***********/
            services.AddTransient<ILogManager, LogManager>();
            /**********Managers***********/

            return services;
        }
    }
}