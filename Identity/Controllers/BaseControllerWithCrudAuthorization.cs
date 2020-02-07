using System;
using System.Threading.Tasks;
using AutoMapper;
using CustomFramework.BaseWebApi.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CustomFramework.BaseWebApi.Utils.Business;
using CustomFramework.BaseWebApi.Data.Models;
using CustomFramework.BaseWebApi.Utils.Utils.Exceptions;
using CustomFramework.BaseWebApi.Utils.Contracts;

namespace CustomFramework.BaseWebApi.Identity.Controllers
{
    public abstract class BaseControllerWithCrudAuthorization<TEntity, TCreateRequest, TUpdateRequest, TResponse, TManager, TKey>
         : BaseControllerWithCrdAuthorization<TEntity, TCreateRequest, TResponse, TManager, TKey>
         where TEntity : BaseModel<TKey>
         where TManager : IBusinessManager<TEntity, TCreateRequest, TKey>, IBusinessManagerUpdate<TEntity, TUpdateRequest, TKey>
    {
        protected BaseControllerWithCrudAuthorization(ILocalizationService localizationService, ILogger<Controller> logger, IMapper mapper, TManager manager)
            : base(localizationService, logger, mapper, manager)
        {

        }

        protected Task<IActionResult> BaseUpdateAsync(TKey id, [FromBody] TUpdateRequest request)
        {
            return CommonOperationAsync<IActionResult>(async () =>
            {
                if (!ModelState.IsValid)
                    throw new ArgumentException(ModelState.ModelStateToString(LocalizationService));

                var result = await Manager.UpdateAsync(id, request);
                return Ok(new ApiResponse(LocalizationService, Logger).Ok(Mapper.Map<TEntity, TResponse>(result)));
            });
        }
    }
}