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

namespace CustomFramework.BaseWebApi.Controllers
{
    public abstract class BaseControllerWithCrud<TEntity, TCreateRequest, TUpdateRequest, TResponse, TManager, TKey>
        : BaseControllerWithCrd<TEntity, TCreateRequest, TResponse, TManager, TKey>
        where TEntity : BaseModel<TKey>
        where TManager : IBusinessManager<TEntity, TCreateRequest, TKey>, IBusinessManagerUpdate<TEntity, TUpdateRequest, TKey>
    {
        protected BaseControllerWithCrud(ILocalizationService localizationService, ILogger<Controller> logger, IMapper mapper, TManager manager) : base(localizationService, logger, mapper, manager)
        {

        }

        public async virtual Task<IActionResult> Update(TKey id, [FromBody] TUpdateRequest request)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException(ModelState.ModelStateToString(LocalizationService));
                
            var result = await (CommonOperationAsync<TEntity>(async () =>
            {
                return await Manager.UpdateAsync(id, request);
            }));

            return Ok(new ApiResponse(LocalizationService, Logger).Ok(Mapper.Map<TEntity, TResponse>(result)));
        }
    }
}