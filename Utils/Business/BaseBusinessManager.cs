using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CustomFramework.BaseWebApi.Utils.Constants;
using CustomFramework.BaseWebApi.Utils.Enums;
using CustomFramework.BaseWebApi.Utils.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomFramework.BaseWebApi.Utils.Business
{
    public abstract class BaseBusinessManager
    {
        private readonly ILogger<BaseBusinessManager> _logger;
        protected readonly IMapper Mapper;
        private readonly int _userId;

        protected BaseBusinessManager(ILogger<BaseBusinessManager> logger, IMapper mapper)
        {
            _logger = logger;
            Mapper = mapper;
        }

        protected BaseBusinessManager(ILogger<BaseBusinessManager> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            Mapper = mapper;

            if (httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                _userId = Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        protected async Task<T> CommonOperationAsync<T>(Func<Task<T>> func, BusinessBaseRequest businessBaseRequest = null)
        {
            try
            {
                var result = await func.Invoke();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"{DefaultResponseMessages.AnErrorHasOccured} - {ex.Message}");
                throw;
            }
        }

        protected async Task<T> CommonOperationAsync<T>(Func<Task<T>> func, BusinessUtilMethod businessUtilMethod, string additionalInfo)
        {
            try
            {
                var result = await func.Invoke();
                BusinessUtil.Execute(businessUtilMethod, result, additionalInfo);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"{DefaultResponseMessages.AnErrorHasOccured} - {ex.Message}");
                throw;
            }
        }

        protected async Task<T> CommonOperationAsync<T>(Func<Task<T>> func, BusinessBaseRequest businessBaseRequest, BusinessUtilMethod businessUtilMethod, string additionalInfo)
        {
            try
            {
                var result = await func.Invoke();
                BusinessUtil.Execute(businessUtilMethod, result, additionalInfo);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"{DefaultResponseMessages.AnErrorHasOccured} - {ex.Message}");
                throw;
            }
        }

        protected async Task CommonOperationAsync(Func<Task> func, BusinessBaseRequest businessBaseRequest = null)
        {
            try
            {
                await func.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"{DefaultResponseMessages.AnErrorHasOccured} - {ex.Message}");
                throw;
            }
        }

        protected T CommonOperation<T>(Func<T> func, BusinessBaseRequest businessBaseRequest, BusinessUtilMethod businessUtilMethod, string additionalInfo)
        {
            try
            {
                var result = func.Invoke();
                BusinessUtil.Execute(businessUtilMethod, result, additionalInfo);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"{DefaultResponseMessages.AnErrorHasOccured} - {ex.Message}");
                throw;
            }
        }

        protected T CommonOperation<T>(Func<T> func, BusinessBaseRequest businessBaseRequest)
        {
            try
            {
                var result = func.Invoke();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, $"{ex.Message}");
                throw;
            }
        }

        protected int GetUserId()
        {
            if (_userId == 0) throw new Exception($"UserIdNullError");
            return _userId;
        }
    }
}