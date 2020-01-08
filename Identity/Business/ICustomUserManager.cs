using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomFramework.BaseWebApi.Contracts.ApiContracts;
using CustomFramework.BaseWebApi.Data.Contracts;
using CustomFramework.BaseWebApi.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace CustomFramework.BaseWebApi.Identity.Business
{
    public interface ICustomUserManager<TUser>
        where TUser : CustomUser
    {
        Task<IdentityResult> RegisterAsync(TUser user, string password, int createUserId, Func<Task> func = null);
        Task<IdentityResult> RegisterAsync(TUser user, string password, List<string> roles, int createUserId, Func<Task> func = null);
        Task<IdentityResult> RegisterWithGeneratedPasswordAsync(TUser user, string password, List<string> roles, int generatePasswordLength, int createUserId, Func<Task> func = null);
        Task<IdentityResult> ChangePasswordWithEmailAsync(string email, string oldPassword, string newPassword, string confirmPassword);
        Task<IdentityResult> ChangePasswordWithUserNameAsync(string userName, string oldPassword, string newPassword, string confirmPassword);
        Task<IdentityResult> AccessFailedAsync(int id);
        Task<IdentityResult> ResetAccessFailedCountAsync(int id);
        Task<IdentityResult> AddClaimAsync(int id, Claim claim, IList<Claim> existingClaims);
        Task<IList<Claim>> AddClaimsAsync(int id, IEnumerable<Claim> claims, IList<Claim> existingClaims);
        Task<IdentityResult> AddToRoleAsync(int id, string role);
        Task<IdentityResult> AddToRolesAsync(int id, IEnumerable<string> roles);
        Task<IdentityResult> ChangeEmailAsync(int id, string newEmail, string token);
        Task<IdentityResult> ChangePasswordAsync(int id, string currentPassword, string newPassword);
        Task<IdentityResult> ConfirmEmailAsync(int id, string token);
        Task<IdentityResult> CreateAsync(TUser user, string password, int createUserId, Func<Task> func = null);
        Task<IdentityResult> DeleteAsync(int id, int deleteUserId, Func<Task> deleteCheck = null);
        Task<IdentityResult> SetPassiveAsync(int id, int operatorUserId, Func<Task> passiveCheck = null);
        Task<TUser> FindByIdAsync(string id);
        Task<TUser> FindByEmailAsync(string email);
        Task<TUser> FindByUserNameAsync(string userName);
        Task<string> GenerateTokenForChangeEmailAsync(TUser user, string newEmail);
        Task<TUser> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        Task<TUser> GetByEmailAsync(string email);
        Task<TUser> GetByUserNameAsync(string userName);
        Task<IList<Claim>> GetUserClaimsAsync(int id);
        Task<IList<Claim>> GetAllClaimsForLoggedUserAsync();
        Task<IList<TUser>> GetUsersInRoleAsync(string roleName);
        Task<IList<string>> GetRolesAsync(int id);
        Task<string> GenerateEmailConfirmationTokenAsync(TUser user);
        Task<string> GeneratePasswordResetTokenAsync(TUser user);
        Task<IList<TUser>> GetAllAsync();
        Task<TUser> GetByIdAsync(int id);
        Task<bool> IsEmailConfirmedAsync(TUser user);
        Task<IdentityResult> RemoveClaimAsync(int id, Claim claim);
        Task<IdentityResult> RemoveFromRoleAsync(int id, string role);
        Task<IdentityResult> RemoveFromRolesAsync(int id, IEnumerable<string> roles);
        Task<IdentityResult> ResetPasswordAsync(int id, string token, string newPassword);
        Task<IdentityResult> ResetPasswordAsync(string emailAddress, string token, string newPassword, string confirmPassword, string emailTitle, string emailText, bool emailBodyIsHtml);
        Task<IdentityResult> UpdateAsync(TUser user, int updateUserId);
        Task<ICustomList<TUser>> GetOnlineUsers(int sessionMinutes, int pageIndex, int pageSize, DateTime? DateTimeNowValue = null);
        IdentityOptions GetOptions();
    }
}