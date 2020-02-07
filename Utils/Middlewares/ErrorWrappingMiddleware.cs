using CustomFramework.BaseWebApi.Utils.Contracts;
using CustomFramework.BaseWebApi.Utils.Utils.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Utils.Constants;
using CustomFramework.BaseWebApi.Resources;

namespace CustomFramework.BaseWebApi.Utils.Middlewares
{
    public class ErrorWrappingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorWrappingMiddleware> _logger;
        private readonly ILocalizationService _localizationService;

        public ErrorWrappingMiddleware(RequestDelegate next, ILogger<ErrorWrappingMiddleware> logger, ILocalizationService localizationService)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger;
            _localizationService = localizationService;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the http status code middleware will not be executed.");
                    throw;
                }

                var errorMessage = ex.Message;

                if (ex.InnerException != null) errorMessage += $"-- {ex.InnerException.Message}";

                //if (ex.Source.StartsWith("Npgsql.EntityFrameworkCore") && ex.InnerException is TimeoutException)
                //{
                //    errorMessage = DefaultResponseMessages.DbConnectionError;
                //}

                if(errorMessage.Contains("is not allowed to connect to this MySQL server"))
                {
                    errorMessage = DefaultResponseMessages.DbConnectionError;
                }

                context.Response.StatusCode = (int)ex.ExceptionToStatusCode();
                context.Response.ContentType = "application/json";

                var apiResponse = new ApiResponse(_localizationService, _logger).Error((HttpStatusCode)context.Response.StatusCode, errorMessage, ex);

                _logger.LogError(0, ex, errorMessage);

                await context.Response.WriteAsync(apiResponse.ToString());
            }
        }
    }
}