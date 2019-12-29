using System.Collections.Generic;
using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Authorization.Attributes;

namespace CustomFramework.BaseWebApi.Identity.Business
{
    public interface IPermissionManager 
    {
        Task<bool> HasPermission(IEnumerable<PermissionAttribute> attributes, IList<string> roles);
        Task<bool> HasPermission(IEnumerable<PermissionAttribute> attributes);
    }

}