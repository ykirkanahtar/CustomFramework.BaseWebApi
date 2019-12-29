using CustomFramework.BaseWebApi.Utils.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CustomFramework.BaseWebApi.Utils.Extensions
{
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
