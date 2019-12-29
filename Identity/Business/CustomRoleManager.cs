using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CustomFramework.BaseWebApi.Identity.Models;
using CustomFramework.BaseWebApi.Identity.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CustomFramework.BaseWebApi.Utils.Business;
using CustomFramework.BaseWebApi.Resources;
using CustomFramework.BaseWebApi.Utils.Utils;
using CustomFramework.BaseWebApi.Contracts;

namespace CustomFramework.BaseWebApi.Identity.Business
{
    public class CustomRoleManager<TUser, TRole> : BaseBusinessManager, ICustomRoleManager<TRole>
        where TUser : CustomUser
    where TRole : CustomRole
    {
        private readonly RoleManager<TRole> _roleManager;
        private readonly UserManager<TUser> _userManager;
        public CustomRoleManager(RoleManager<TRole> roleManager, UserManager<TUser> userManager, ILogger<CustomRoleManager<TUser, TRole>> logger, IMapper mapper) : base(logger, mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddClaimAsync(int id, Claim claim, IList<Claim> existingClaims)
        {
            var role = await GetByIdAsync(id);
            var claims = await GetClaimsAsync(role.Name);

            ClaimChecker.CheckClaimStatus(claim, claims, existingClaims);

            return await _roleManager.AddClaimAsync(role, claim);
        }

        public async Task<IList<Claim>> AddClaimsAsync(int id, IList<Claim> claims, IList<Claim> existingClaims)
        {
            var addedClaims = new List<Claim>();
            var role = await GetByIdAsync(id);
            var claimsInRole = await GetClaimsAsync(role.Name);

            foreach (var claim in claims)
            {
                Claim checkedClaim;
                bool claimCheckSuccess = false;
                try
                {
                    //CheckClaimStaus metodu hata oluştuğunda Exception fırlatıyor.
                    //Fakat bu metotta claim değerleri bir dizi halinde olduğu için 
                    //Başarısız işlemlerde hata üretilmesini değil, diziye eklenmemesini istiyoruz
                    //Bu yüzden boş bir try catch bloğu içerisine alındı
                    checkedClaim = ClaimChecker.CheckClaimStatus(claim, claimsInRole, existingClaims);
                    claimCheckSuccess = true;
                }
                catch (KeyNotFoundException)
                {
                    throw new KeyNotFoundException(nameof(Claim));
                }
                catch { }

                if (claimCheckSuccess)
                {
                    await _roleManager.AddClaimAsync(role, claim);
                    addedClaims.Add(claim);
                }
            }

            return addedClaims;
        }

        public async Task<IdentityResult> CreateAsync(TRole role)
        {
            role.Status = Status.Active;
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> DeleteAsync(int id)
        {
            var role = await GetByIdAsync(id);

            (await _userManager.GetUsersInRoleAsync(role.Name)).Where(p => p.Status == Status.Active).CheckSubFieldIsExistForDelete("User");

            var claims = await GetClaimsAsync(role.Name);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            var uniqueValue = DateTime.UtcNow.ToString();

            role.Name = $"deleted_{uniqueValue}_{role.Name}"; //Identity'de silinen bir veriye ait unique alan tekrar kaydedilmek istendiğinde duplicate key hatası veriyordu. Buna önlem olarak silinen kaydın unique key alanına unique değerler getirildi
            role.NormalizedName = role.Name.ToUpper();

            role.Status = Status.Deleted;
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<TRole> FindByIdAsync(string id)
        {
            return await _roleManager.FindByIdAsync(id);
        }

        public async Task<TRole> FindByNameAsync(string name)
        {
            return await _roleManager.FindByNameAsync(name);
        }

        public async Task<TRole> GetByIdAsync(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null || role.Status != Status.Active) throw new KeyNotFoundException("Role");
            return role;
        }

        public async Task<TRole> GetByNameAsync(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            if (role == null || role.Status != Status.Active) throw new KeyNotFoundException("Role");
            return role;
        }

        public async Task<IdentityResult> UpdateAsync(int id, TRole role)
        {
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IList<TRole>> GetAllAsync()
        {
            return await _roleManager.Roles.AsQueryable().Where(p => p.Status == Status.Active).ToListAsync();
        }

        public async Task<IList<Claim>> GetClaimsAsync(string name)
        {
            var role = await GetByNameAsync(name);
            return await _roleManager.GetClaimsAsync(role);
        }

        public async Task<IdentityResult> RemoveClaimAsync(int id, Claim claim)
        {
            var role = await GetByIdAsync(id);
            return await _roleManager.RemoveClaimAsync(role, claim);
        }

        public async Task<IList<Claim>> RemoveClaimsAsync(int id, IList<Claim> claims)
        {
            var removedClaims = new List<Claim>();
            var role = await GetByIdAsync(id);
            var claimsInRole = await GetClaimsAsync(role.Name);

            foreach (var claim in claims)
            {
                var claimIsExist = (from p in claimsInRole where p.Type == claim.Type && p.Value == claim.Value select p).Count() > 0;

                if (claimIsExist)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                    removedClaims.Add(claim);
                }
            }
            return removedClaims;
        }
    }
}