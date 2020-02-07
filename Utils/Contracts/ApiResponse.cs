using CustomFramework.BaseWebApi.Utils.Constants;
using CustomFramework.BaseWebApi.Utils.Utils.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using CustomFramework.BaseWebApi.Contracts.ApiContracts;
using CustomFramework.BaseWebApi.Resources;
using CustomFramework.BaseWebApi.Data.Contracts;

namespace CustomFramework.BaseWebApi.Utils.Contracts
{
    public class ApiResponse : WebApiResponse, IApiResponse
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;

        public ApiResponse(ILocalizationService localizationService, ILogger logger)
        {
            _localizationService = localizationService;
            _logger = logger;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        [JsonConstructor]
        public ApiResponse(HttpStatusCode statusCode, string message, object result, int totalCount, ErrorResponse errorResponse)
        {
            StatusCode = statusCode;
            Message = message;
            Result = result;
            TotalCount = totalCount;
            ErrorResponse = errorResponse;
        }



        public ApiResponse Ok(object result)
        {
            StatusCode = HttpStatusCode.OK;
            Result = result;
            TotalCount = 1;
            Message = _localizationService.GetValue(DefaultResponseMessages.SuccessMessage);

            //_logger.LogInformation(Message, Result);
            return this;
        }

        public ApiResponse Ok<T>(ICustomList<T> list) where T : class
        {
            StatusCode = HttpStatusCode.OK;
            Result = list;
            TotalCount = list.TotalCount;
            Message = _localizationService.GetValue(DefaultResponseMessages.SuccessMessage);

            //_logger.LogInformation(Message, Result);
            return this;
        }

        public ApiResponse Ok<T>(IEnumerable<T> list, int totalCount)
        {
            StatusCode = HttpStatusCode.OK;
            Result = list;
            TotalCount = totalCount;
            Message = _localizationService.GetValue(DefaultResponseMessages.SuccessMessage);

            //_logger.LogInformation(Message, Result);
            return this;
        }

        public ApiResponse Error(HttpStatusCode statusCode, Exception exception)
        {
            ErrorResponse = new ErrorResponse(exception);
            StatusCode = statusCode;
            Message = $"{GetDefaultMessageForException(exception)}";

            //_logger.LogError(0, exception, Message);
            return this;
        }

        public ApiResponse Error(HttpStatusCode statusCode, string message, Exception exception)
        {
            ErrorResponse = new ErrorResponse(
                exception
            );

            StatusCode = statusCode;
            Message = $"{GetDefaultMessageForException(exception)} : {_localizationService.GetValue(message)}";

            //_logger.LogError(0, exception, Message);
            return this;
        }

        public ApiResponse ModelStateError(ModelStateDictionary modelState)
        {
            var modelstateString = modelState.ModelStateToString(_localizationService);


            ErrorResponse = new ErrorResponse(
                new ValidationException()
                , modelstateString
            );

            StatusCode = HttpStatusCode.BadRequest;
            Message = _localizationService.GetValue(DefaultResponseMessages.ModelStateErrors);

            //_logger.LogError(0, Message);
            return this;
        }

        private string GetDefaultMessageForException(Exception exception)
        {

            var message = $"{_localizationService.GetValue(exception.Message)}";
            if (exception.InnerException != null) message += $"-- {_localizationService.GetValue(exception.InnerException.Message)}";

            if (message.Contains("See the inner exception for details"))
                if (exception.InnerException != null)
                    message = exception.InnerException.Message;

            if (message.Contains("See the inner exception for details"))
            {
                if (exception.InnerException?.InnerException != null)
                    message = exception.InnerException.InnerException.Message;
            }

            if (message.Contains("See the inner exception for details"))
            {
                if (exception.InnerException?.InnerException?.InnerException != null)
                    message = exception.InnerException.InnerException.InnerException.Message;
            }

            var returnMessage = new ExceptionOperation(exception).GetReturnMessage(ref message);

            var localizatedReturnMessage = _localizationService.GetValue(returnMessage);
            return localizatedReturnMessage == string.Empty
                ? $"{_localizationService.GetValue(message)}" : $"{localizatedReturnMessage}";
        }
    }
}