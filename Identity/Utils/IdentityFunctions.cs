using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CustomFramework.BaseWebApi.Identity.Utils
{
    public static class IdentityFunctions
    {
        public static int GetLoggedUserId(IHttpContextAccessor httpContextAccessor)
        {
            return Convert.ToInt32(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}