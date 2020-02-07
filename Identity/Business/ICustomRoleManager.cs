using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace CustomFramework.BaseWebApi.Identity.Business
{
    public interface ICustomRoleManager<TRole> where TRole : CustomRole
    {
        Task<IdentityResult> AddClaimAsync(int id, Claim claim, IList<Claim> existingClaims);
        Task<IList<Claim>> AddClaimsAsync(int id, IList<Claim> claims, IList<Claim> existingClaims);
        Task<TRole> FindByIdAsync(string id);
        Task<TRole> FindByNameAsync(string name);
        Task<IdentityResult> CreateAsync(TRole role);
        Task<IdentityResult> UpdateAsync(int id, TRole role);
        Task<IdentityResult> DeleteAsync(int id);
        Task<TRole> GetByIdAsync(int id);
        Task<TRole> GetByNameAsync(string name);
        Task<IList<TRole>> GetAllAsync();
        Task<IList<Claim>> GetClaimsAsync(string name);
        Task<IdentityResult> RemoveClaimAsync(int id, Claim claim);
        Task<IList<Claim>> RemoveClaimsAsync(int id, IList<Claim> claims);

    }
}