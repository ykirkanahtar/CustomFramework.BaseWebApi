using System.Threading.Tasks;
using AutoMapper;
using CustomFramework.BaseWebApi.Resources;
using CustomFramework.BaseWebApi.Contracts.Requests;
using CustomFramework.BaseWebApi.Contracts.Responses;
using CustomFramework.BaseWebApi.Identity.Business;
using CustomFramework.BaseWebApi.Identity.Models;
using CustomFramework.BaseWebApi.Utils.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CustomFramework.BaseWebApi.Identity.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BaseClientApplicationController
        : BaseControllerWithCrdAuthorization<ClientApplication, ClientApplicationRequest, ClientApplicationResponse, IClientApplicationManager, int>
    {
        public BaseClientApplicationController(ILocalizationService localizationService, ILogger<Controller> logger, IMapper mapper, IClientApplicationManager manager) 
            : base(localizationService, logger, mapper, manager)
        {

        }

        [Route("create")]
        [HttpPost]
        public async virtual Task<IActionResult> CreateAsync([FromBody] ClientApplicationRequest request)
        {
            return await BaseCreateAsync(request);
        }

        [Route("delete/{id:int}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            return await BaseDeleteAsync(id);
        }

        [Route("get/id/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return await BaseGetByIdAsync(id);
        }

        [Route("{id:int}/update")]
        [HttpPut]
        public Task<IActionResult> UpdateClientApplicationNameAsync(int id, [FromBody] ClientApplicationUpdateRequest request)
        {
            return CommonOperationAsync<IActionResult>(async () =>
            {
                var result = await Manager.UpdateAsync(id, request);
                return Ok(new ApiResponse(LocalizationService, Logger).Ok(
                    Mapper.Map<ClientApplication, ClientApplicationResponse>(result)));
            });
        }

        [Route("{id:int}/update/clientApplicationPassword")]
        [HttpPut]
        public Task<IActionResult> UpdateClientApplicationPasswordAsync(int id, [FromBody] string clientApplicationPassword)
        {
            return CommonOperationAsync<IActionResult>(async () =>
            {
                var result = await Manager.UpdateClientApplicationPasswordAsync(id, clientApplicationPassword);
                return Ok(new ApiResponse(LocalizationService, Logger).Ok(
                    Mapper.Map<ClientApplication, ClientApplicationResponse>(result)));
            });
        }

        [Route("get/clientapplicationcode/{clientApplicationCode}")]
        [HttpGet]
        public Task<IActionResult> GetByClientApplicationCodeAsync(string clientApplicationCode)
        {
            return CommonOperationAsync<IActionResult>(async () =>
            {
                var result = await Manager.GetByClientApplicationCodeAsync(clientApplicationCode);
                return Ok(new ApiResponse(LocalizationService, Logger).Ok(
                    Mapper.Map<ClientApplication, ClientApplicationResponse>(result)));
            });
        }
    }
       
}