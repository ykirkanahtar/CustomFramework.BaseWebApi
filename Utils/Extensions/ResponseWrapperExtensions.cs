using CustomFramework.BaseWebApi.Utils.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CustomFramework.BaseWebApi.Utils.Extensions
{
    public static class ResponseWrapperExtensions
    {
        public static IApplicationBuilder UseErrorWrappingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorWrappingMiddleware>();
        }
    }
}