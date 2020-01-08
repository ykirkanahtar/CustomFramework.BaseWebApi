using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CustomFramework.Utils;
using CustomFramework.BaseWebApi.Identity.Constants;
using CustomFramework.BaseWebApi.Identity.Extensions;
using CustomFramework.BaseWebApi.Identity.Models;
using CustomFramework.BaseWebApi.Identity.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CustomFramework.BaseWebApi.Identity.Data.Repositories;
using CustomFramework.EmailProvider;
using CustomFramework.BaseWebApi.Resources;
using CustomFramework.BaseWebApi.Utils.Utils;
using CustomFramework.BaseWebApi.Utils.Business;
using CustomFramework.BaseWebApi.Contracts;
using CustomFramework.BaseWebApi.Contracts.ApiContracts;

namespace CustomFramework.BaseWebApi.Identity.Business
{
    public class CustomUserManager<TUser, TRole> : BaseBusinessManager, ICustomUserManager<TUser>
        where TUser : CustomUser
    where TRole : CustomRole
    {
        private readonly UserManager<TUser> _userManager;
        private readonly ICustomRoleManager<TRole> _roleManager;
        private readonly IIdentityModel _identityModel;
        private readonly IEmailSender _emailSender;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomUserRepository<TUser> _customUserRepository;


        public CustomUserManager(ILocalizationService localizationService, ICustomUserRepository<TUser> customUserRepository, UserManager<TUser> userManager, ICustomRoleManager<TRole> roleManager, IIdentityModel identityModel, IEmailSender emailSender, ILogger<CustomRoleManager<TUser, TRole>> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(logger, mapper, httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _identityModel = identityModel;
            _emailSender = emailSender;
            _localizationService = localizationService;
            _customUserRepository = customUserRepository;
        }

        public async Task<IdentityResult> RegisterAsync(TUser user, string password, int createUserId, Func<Task> func = null)
        {
            return await CreateAsync(user, password, createUserId, func);
        }

        public async Task<IdentityResult> RegisterAsync(TUser user, string password, List<string> roles, int createUserId, Func<Task> func = null)
        {
            var result = await RegisterAsync(user, password, createUserId, func);
            if (!result.Succeeded) return result;

            var addToRoleResult = await AddToRolesAsync(user.Id, roles);
            if (!result.Succeeded) return result;

            return result;
        }

        public async Task<IdentityResult> RegisterWithGeneratedPasswordAsync(TUser user, string password, List<string> roles, int generatePasswordLength, int createUserId, Func<Task> func = null)
        {
            var passwordLength = generatePasswordLength < 6 ? 6 : (int)generatePasswordLength;
            var passwordGenerated = Password.Generate(passwordLength, 1);
            password = passwordGenerated;

            return await RegisterAsync(user, password, roles, createUserId, func);
        }

        public async Task<IdentityResult> ChangePasswordWithEmailAsync(string email, string oldPassword, string newPassword, string confirmPassword)
        {
            var user = await GetByEmailAsync(email);
            if (user == null)
            {
                throw new KeyNotFoundException($"{IdentityStringMessages.User}");
            }

            return await BaseChangePasswordAsync(user, oldPassword, newPassword, confirmPassword);
        }

        public async Task<IdentityResult> ChangePasswordWithUserNameAsync(string userName, string oldPassword, string newPassword, string confirmPassword)
        {
            var user = await GetByUserNameAsync(userName);
            if (user == null)
            {
                throw new KeyNotFoundException($"{IdentityStringMessages.User}");
            }

            return await BaseChangePasswordAsync(user, oldPassword, newPassword, confirmPassword);
        }

        private async Task<IdentityResult> BaseChangePasswordAsync(TUser user, string oldPassword, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                throw new ArgumentException($"{IdentityStringMessages.PasswordAndConfirmPasswordDoesntMatch}");
            }

            if (oldPassword == newPassword)
            {
                throw new ArgumentException($"{IdentityStringMessages.OldPasswordAndNewPasswordCanNotBeSame}");
            }

            return await ChangePasswordAsync(user.Id, oldPassword, newPassword);
        }

        public async Task<IdentityResult> AccessFailedAsync(int id)
        {
            var user = await GetByIdAsync(id);
            return await _userManager.AccessFailedAsync(user);
        }

        public async Task<IdentityResult> ResetAccessFailedCountAsync(int id)
        {
            var user = await GetByIdAsync(id);
            return await _userManager.ResetAccessFailedCountAsync(user);
        }        

        public async Task<IdentityResult> AddClaimAsync(int id, Claim claim, IList<Claim> existingClaims)
        {
            var user = await GetByIdAsync(id);
            var claims = await GetUserClaimsAsync(user.Id);

            ClaimChecker.CheckClaimStatus(claim, claims, existingClaims);

            return await _userManager.AddClaimAsync(user, claim);
        }

        public async Task<IList<Claim>> AddClaimsAsync(int id, IEnumerable<Claim> claims, IList<Claim> existingClaims)
        {
            var addedClaims = new List<Claim>();
            var user = await GetByIdAsync(id);
            var userClaims = await GetUserClaimsAsync(user.Id);

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
                    checkedClaim = ClaimChecker.CheckClaimStatus(claim, userClaims, existingClaims);
                    claimCheckSuccess = true;
                }
                catch (KeyNotFoundException)
                {
                    throw new KeyNotFoundException(nameof(Claim));
                }
                catch { }

                if (claimCheckSuccess)
                {
                    await _userManager.AddClaimAsync(user, claim);
                    addedClaims.Add(claim);
                }
            }

            return addedClaims;
        }

        public async Task<IdentityResult> AddToRoleAsync(int id, string role)
        {
            var user = await GetByIdAsync(id);
            await _roleManager.GetByNameAsync(role);
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> AddToRolesAsync(int id, IEnumerable<string> roles)
        {
            var user = await GetByIdAsync(id);
            foreach (var role in roles)
            {
                await _roleManager.GetByNameAsync(role);
            }
            return await _userManager.AddToRolesAsync(user, roles);
        }
        public async Task<IdentityResult> ChangeEmailAsync(int id, string newEmail, string token)
        {
            var user = await GetByIdAsync(id);
            return await _userManager.ChangeEmailAsync(user, newEmail, token);
        }

        public async Task<IdentityResult> ChangePasswordAsync(int id, string currentPassword, string newPassword)
        {
            var user = await GetByIdAsync(id);
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<bool> CheckPasswordAsync(int id, string password)
        {
            var user = await GetByIdAsync(id);
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(int id, string token)
        {
            var user = await GetByIdAsync(id);

            var codeDecodedBytes = WebEncoders.Base64UrlDecode(token);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);

            return await _userManager.ConfirmEmailAsync(user, codeDecoded);
        }

        public async Task<IdentityResult> CreateAsync(TUser user, string password, int createUserId, Func<Task> func = null)
        {
            user.Status = Status.Active;
            user.CreateDateTime = DateTime.Now;
            user.CreateUserId = createUserId;
            if (func != null) await func.Invoke();
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> DeleteAsync(int id, int deleteUserId, Func<Task> deleteCheck = null)
        {
            var user = await GetByIdAsync(id);

            await deleteCheck.Invoke();

            var roles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, roles);

            (await GetRolesAsync(id)).CheckSubFieldIsExistForDelete("Role");

            var uniqueValue = DateTime.UtcNow.ToString();
            var emailValue = $"deleted_{uniqueValue}_{user.Email}";
            var userNameValue = $"{DateTime.UtcNow.ToString("yyMMdd")}_{Guid.NewGuid().ToString().Substring(0, 6)}";

            user.Email = emailValue; //Identity'de silinen bir veriye ait unique alan tekrar kaydedilmek istendiğinde duplicate key hatası veriyordu. Buna önlem olarak silinen kaydın unique key alanına unique değerler getirildi
            user.NormalizedEmail = emailValue.ToUpper();

            user.UserName = userNameValue; //user.Email ile aynı değeri alınca hata vermiyor fakat entity'i de güncellemiyordu. Bu yüzden kısa bir değer seçildi

            user.Status = Status.Deleted;
            user.DeleteDateTime = DateTime.Now;
            user.DeleteUserId = deleteUserId;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> SetPassiveAsync(int id, int operatorUserId, Func<Task> passiveCheck = null)
        {
            var user = await GetByIdAsync(id);

            await passiveCheck.Invoke();

            var roles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, roles);

            (await GetRolesAsync(id)).CheckSubFieldIsExistForDelete("Role");

            user.Status = Status.Passive;
            user.UpdateDateTime = DateTime.Now;
            user.UpdateUserId = operatorUserId;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<string> GenerateTokenForChangeEmailAsync(TUser user, string newEmail)
        {
            return await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        }

        public async Task<TUser> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.Status != Status.Active) return null;
            return user;
        }

        public async Task<TUser> FindByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || user.Status != Status.Active) return null;
            return user;
        }

        public async Task<TUser> GetByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.Status != Status.Active) throw new KeyNotFoundException("User");
            return user;
        }

        public async Task<TUser> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await _userManager.GetUserAsync(claimsPrincipal);
        }

        public async Task<TUser> GetByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null || user.Status != Status.Active) throw new KeyNotFoundException("User");
            return user;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(TUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(TUser user)
        {
            await GetByIdAsync(user.Id);
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IList<string>> GetRolesAsync(int id)
        {
            var user = await GetByIdAsync(id);
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<TUser> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IList<TUser>> GetAllAsync()
        {
            return await _userManager.Users.AsQueryable().Where(p => p.Status == Status.Active).ToListAsync();
        }

        public async Task<TUser> GetByIdAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null || user.Status != Status.Active) throw new KeyNotFoundException("User");
            return user;
        }

        public async Task<IList<Claim>> GetUserClaimsAsync(int id)
        {
            var user = await GetByIdAsync(id);
            return await _userManager.GetClaimsAsync(user);
        }

        public async Task<IList<Claim>> GetAllClaimsForLoggedUserAsync()
        {
            var userId = GetUserId();
            var userClaims = await GetUserClaimsAsync(userId);
            var claims = userClaims;

            var roles = await GetRolesAsync(userId);
            foreach (var role in roles)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var roleClaim in roleClaims)
                {
                    //UserClaim RoleClaiim'i ezdiği için öncelikle UserClaim'de yetki var mı diye kontrol ediliyor.
                    var claimIsExistInUserClaims = (from p in userClaims where p.Type == roleClaim.Type select p).Count() > 0;

                    if (!claimIsExistInUserClaims)
                    {
                        //Çift kaydı engellemek için ClaimType ve ClaimValue değerleri eşleşen kayıtlar eklenmiyor.
                        var claimIsExistInClaims = (from p in claims where p.Type == roleClaim.Type && p.Value == roleClaim.Value select p).Count() > 0;

                        if (!claimIsExistInClaims)
                            claims.Add(roleClaim);
                    }
                }
            }

            return claims;
        }

        public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users.Where(p => p.Status == Status.Active).ToList();
        }

        public async Task<bool> IsEmailConfirmedAsync(TUser user)
        {
            await GetByIdAsync(user.Id);
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IdentityResult> RemoveClaimAsync(int id, Claim claim)
        {
            var user = await GetByIdAsync(id);
            return await _userManager.RemoveClaimAsync(user, claim);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(int id, string role)
        {
            var user = await GetByIdAsync(id);
            await _roleManager.GetByNameAsync(role);
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveFromRolesAsync(int id, IEnumerable<string> roles)
        {
            var user = await GetByIdAsync(id);
            foreach (var role in roles)
            {
                await _roleManager.GetByNameAsync(role);
            }
            return await _userManager.RemoveFromRolesAsync(user, roles);
        }

        public async Task<IdentityResult> ResetPasswordAsync(int id, string token, string newPassword)
        {
            var user = await GetByIdAsync(id);
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<IdentityResult> ResetPasswordAsync(string emailAddress, string token, string newPassword, string confirmPassword, string emailTitle, string emailText, bool emailBodyIsHtml)
        {
            if (token == null)
            {
                throw new ArgumentException($"{IdentityStringMessages.CodeIsNeededForCompleteRegistration}"); //A code must be supplied for password reset.
            }

            if (newPassword != confirmPassword)
            {
                throw new ArgumentException($"{IdentityStringMessages.PasswordAndConfirmPasswordDoesntMatch}");
            }

            var user = await GetByEmailAsync(emailAddress);

            var codeDecodedBytes = WebEncoders.Base64UrlDecode(token);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);

            var result = await _userManager.ResetPasswordAsync(user, codeDecoded, newPassword);
            if (!result.Succeeded) return result;

            await _emailSender.SendEmailAsync(
                _identityModel.SenderEmailAddress, new List<string> { emailAddress }, emailTitle, emailText, emailBodyIsHtml);

            return result;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, int updateUserId)
        {
            await GetByIdAsync(user.Id);
            user.UpdateDateTime = DateTime.Now;
            user.UpdateUserId = updateUserId;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<ICustomList<TUser>> GetOnlineUsers(int sessionMinutes, int pageIndex, int pageSize, DateTime? DateTimeNowValue = null)
        {
            return await _customUserRepository.GetOnlineUsers(sessionMinutes, pageIndex, pageSize, DateTimeNowValue);
        }

        public IdentityOptions GetOptions()
        {
            return _userManager.Options;
        }
    }
}