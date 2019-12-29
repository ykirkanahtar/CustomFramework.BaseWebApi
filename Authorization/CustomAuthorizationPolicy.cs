using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace CustomFramework.BaseWebApi.Authorization
{
    public class CustomAuthorizationPolicy
    {
        public string Name { get; set; }
        public IList<IAuthorizationRequirement> AuthorizationRequirements { get; set; }
    }
}