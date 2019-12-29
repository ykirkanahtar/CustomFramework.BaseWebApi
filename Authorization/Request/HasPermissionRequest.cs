using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CustomFramework.BaseWebApi.Authorization.Attributes;

namespace CustomFramework.BaseWebApi.Authorization.Request
{
    public class HasPermissionRequest
    {
        public HasPermissionRequest(int applicationId, IEnumerable<PermissionAttribute> permissionAttributes)
        {
            ApplicationId = applicationId;
            PermissionAttributes = permissionAttributes;
        }

        [Required]
        public int ApplicationId { get; }

        [Required]
        public IEnumerable<PermissionAttribute> PermissionAttributes { get; }
    }

}