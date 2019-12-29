using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using CustomFramework.BaseWebApi.Resources;
using CustomFramework.BaseWebApi.Utils.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CustomFramework.BaseWebApi.Utils.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ILocalizationService LocalizationService;
        protected readonly ILogger<Controller> Logger;
        protected readonly IMapper Mapper;

        protected BaseController(ILocalizationService localizationService, ILogger<Controller> logger, IMapper mapper)
        {
            LocalizationService = localizationService;
            Logger = logger;
            Mapper = mapper;
        }

        protected async Task<T> CommonOperationAsync<T>(Func<Task<T>> func)
        {
            using(var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            }, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var result = await func.Invoke();
                    scope.Complete();
                    return result;
                }
                catch (Exception ex)
                {
                    Logger.LogError(0, ex, $"{DefaultResponseMessages.AnErrorHasOccured} - {ex.Message}");
                    throw;
                }
            }
        }

        protected async Task<T> CommonOperationAsync<T>(Func<Task<T>> func, object request)
        {
            return await CommonOperationAsync(func, new List<object> { request });
        }

        protected async Task<T> CommonOperationAsync<T>(Func<Task<T>> func, List<object> request)
        {
            using(var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            }, TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    var json = JsonConvert.SerializeObject(request);
                    var result = await func.Invoke();
                    scope.Complete();
                    return result;
                }
                catch (Exception ex)
                {
                    Logger.LogError(0, ex, $"{DefaultResponseMessages.AnErrorHasOccured} - {ex.Message}");
                    throw;
                }
            }
        }
    }
}