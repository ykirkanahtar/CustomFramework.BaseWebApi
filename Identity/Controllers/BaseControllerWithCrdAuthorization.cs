using System;
using System.Threading.Tasks;
using AutoMapper;
using CustomFramework.BaseWebApi.Resources;
using CustomFramework.BaseWebApi.Data.Models;
using CustomFramework.BaseWebApi.Utils.Business;
using CustomFramework.BaseWebApi.Utils.Contracts;
using CustomFramework.BaseWebApi.Utils.Controllers;
using CustomFramework.BaseWebApi.Utils.Utils.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomFramework.BaseWebApi.Identity.Controllers
{
    public abstract class BaseControllerWithCrdAuthorization<TEntity, TCreateRequest, TResponse, TManager, TKey> : BaseController
        where TEntity : BaseModel<TKey>
        where TManager : IBusinessManager<TEntity, TCreateRequest, TKey>
    {
        protected readonly TManager Manager;

        protected BaseControllerWithCrdAuthorization(ILocalizationService localizationService, ILogger<Controller> logger, IMapper mapper, TManager manager)
            : base(localizationService, logger, mapper)
        {
            Manager = manager;
        }

        protected Task<IActionResult> BaseCreateAsync([FromBody] TCreateRequest request)
        {
            return CommonOperationAsync<IActionResult>(async () =>
            {
                if (!ModelState.IsValid)
                    throw new ArgumentException(ModelState.ModelStateToString(LocalizationService));

                var result = await CommonOperationAsync(async () => await Manager.CreateAsync(request));
                return Ok(new ApiResponse(LocalizationService, Logger).Ok(Mapper.Map<TEntity, TResponse>(result)));
            });
        }

        protected Task<IActionResult> BaseDeleteAsync(TKey id)
        {
            return CommonOperationAsync<IActionResult>(async () =>
            {
                await Manager.DeleteAsync(id);
                return Ok(new ApiResponse(LocalizationService, Logger).Ok(true));
            });
        }

        protected Task<IActionResult> BaseGetByIdAsync(TKey id)
        {
            return CommonOperationAsync<IActionResult>(async () =>
            {
                var result = await Manager.GetByIdAsync(id);
                return Ok(new ApiResponse(LocalizationService, Logger).Ok(Mapper.Map<TEntity, TResponse>(result)));
            });
        }
    }

}