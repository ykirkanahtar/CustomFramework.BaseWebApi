using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Authorization.Attributes;
using CustomFramework.BaseWebApi.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CustomFramework.BaseWebApi.Identity.Business
{
    public class PermissionManager<TUser, TRole> : IPermissionManager
    where TUser : CustomUser
    where TRole : CustomRole
    {
        private readonly ICustomRoleManager<TRole> _roleManager;
        private readonly ICustomUserManager<TUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PermissionManager<TUser, TRole>> _logger;

        public PermissionManager(ICustomRoleManager<TRole> roleManager, ICustomUserManager<TUser> userManager, IHttpContextAccessor httpContextAccessor, ILogger<PermissionManager<TUser, TRole>> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<bool> HasPermission(IEnumerable<PermissionAttribute> attributes, IList<string> roles)
        {
            if (_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null)
                throw new AuthenticationException();

            try
            {
                foreach (var permissionAttribute in attributes)
                {
                    var roleClaims = new List<Claim>();
                    foreach (var role in roles)
                    {
                        var claims = await _roleManager.GetClaimsAsync(role);
                        roleClaims.AddRange(claims);
                    }
                    foreach (var roleClaim in roleClaims)
                    {
                        if (roleClaim.Type.ToLower() == permissionAttribute.ClaimType.ToLower())
                        {
                            if (String.IsNullOrEmpty(permissionAttribute.ClaimValue))
                            {
                                return true;
                            }
                            else
                            {
                                if (roleClaim.Value.ToLower() == permissionAttribute.ClaimValue.ToLower()) return true;
                                else throw new UnauthorizedAccessException($"{permissionAttribute.ClaimType} - {permissionAttribute.ClaimValue}");
                            }
                        }
                    }

                    throw new UnauthorizedAccessException($"{permissionAttribute.ClaimType} - {permissionAttribute.ClaimValue}");
                }
            }
            catch (KeyNotFoundException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
            throw new UnauthorizedAccessException();
        }

        public async Task<bool> HasPermission(IEnumerable<PermissionAttribute> attributes)
        {
            if (_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null)
                throw new AuthenticationException();

            try
            {
                var userId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var roles = await _userManager.GetRolesAsync(userId);

                foreach (var permissionAttribute in attributes)
                {
                    var userClaims = await _userManager.GetUserClaimsAsync(userId);

                    foreach (var userClaim in userClaims)
                    {
                        if (userClaim.Type.ToLower() == permissionAttribute.ClaimType.ToLower())
                        {
                            if (String.IsNullOrEmpty(permissionAttribute.ClaimValue))
                            {
                                return true;
                            }
                            else
                            {
                                if (userClaim.Value.ToLower() == permissionAttribute.ClaimValue.ToLower()) return true;
                                else throw new UnauthorizedAccessException($"{permissionAttribute.ClaimType}");
                            }
                        }
                    }

                    var roleClaims = new List<Claim>();
                    foreach (var role in roles)
                    {
                        var claims = await _roleManager.GetClaimsAsync(role);
                        roleClaims.AddRange(claims);
                    }
                    foreach (var roleClaim in roleClaims)
                    {
                        if (roleClaim.Type.ToLower() == permissionAttribute.ClaimType.ToLower())
                        {
                            if (String.IsNullOrEmpty(permissionAttribute.ClaimValue))
                            {
                                return true;
                            }
                            else
                            {
                                if (roleClaim.Value.ToLower() == permissionAttribute.ClaimValue.ToLower()) return true;
                                else throw new UnauthorizedAccessException($"{permissionAttribute.ClaimType}");
                            }
                        }
                    }

                    throw new UnauthorizedAccessException($"{permissionAttribute.ClaimType}");
                }
            }
            catch (KeyNotFoundException ex)
            {
                throw new UnauthorizedAccessException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw;
            }
            throw new UnauthorizedAccessException();
        }
    }
}