using System.Collections.Generic;
using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Authorization.Attributes;
using CustomFramework.BaseWebApi.Authorization.Handlers;
using CustomFramework.BaseWebApi.Identity.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CustomFramework.BaseWebApi.Identity.Handlers
{
    public class PermissionAuthorizationHandler : AttributeAuthorizationHandler<PermissionAuthorizationRequirement, PermissionAttribute>
    {
        private readonly ILogger<PermissionAuthorizationHandler> _logger;
        private readonly IPermissionManager _permissionManager;

        public PermissionAuthorizationHandler(ILogger<PermissionAuthorizationHandler> logger, IPermissionManager permissionManager)
        {
            _logger = logger;
            _permissionManager = permissionManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement, IEnumerable<PermissionAttribute> attributes)
        {
            await _permissionManager.HasPermission(attributes);
            context.Succeed(requirement);
        }
    }
}