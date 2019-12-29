using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomFramework.Utils;
using CustomFramework.BaseWebApi.LogProvider.Business;
using CustomFramework.BaseWebApi.LogProvider.Models;

namespace CustomFramework.BaseWebApi.Utils.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context, ILogManager logManager)
        {
            var userId = (context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value).ToNullableInt();

            var requestBodyStream = new MemoryStream();
            var originalRequestBody = context.Request.Body;

            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            var url = UriHelper.GetDisplayUrl(context.Request);
            var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();

            var log = new Log
            {
                LoggedUserId = userId,
                RequestBody = requestBodyText,
                RequestMethod = context.Request.Method,
                RequestUrl = url,
                RequestTime = DateTime.Now,
            };

            requestBodyStream.Seek(0, SeekOrigin.Begin);
            context.Request.Body = requestBodyStream;

            var bodyStream = context.Response.Body;

            var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(responseBodyStream).ReadToEnd();
            var responseLogText = $"RESPONSE BODY: {responseBody}";
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(bodyStream);

            log.ResponseBody = responseLogText;
            log.ResponseTime = DateTime.Now;

            await logManager.CreateAsync(log);

        }
    }
}
