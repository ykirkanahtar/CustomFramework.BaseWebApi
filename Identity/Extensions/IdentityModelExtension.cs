using System.IdentityModel.Tokens.Jwt;
using CustomFramework.EmailProvider;
using CustomFramework.BaseWebApi.Identity.Business;
using CustomFramework.BaseWebApi.Identity.Data.Repositories;
using CustomFramework.BaseWebApi.Identity.Models;
using CustomFramework.BaseWebApi.Identity.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CustomFramework.BaseWebApi.Authorization.Utils;

namespace CustomFramework.BaseWebApi.Identity.Extensions
{
    public static class IdentityModelExtension<TUser, TRole, TContext>
        where TUser : CustomUser
        where TRole : CustomRole
        where TContext : DbContext
    {
        public static IdentityModel IdentityConfig { get; set; }
        public static IServiceCollection AddIdentityModel(IServiceCollection services, IdentityModel identityModel, Token token, bool requireConfirmedEmail)
        {
            IdentityConfig = identityModel;

            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSingleton<IToken, Token>(p => IdentityConfig.Token);
            services.AddSingleton<IEmailConfig, EmailConfig>(p => IdentityConfig.EmailConfig);
            services.AddSingleton<IIdentityModel, IdentityModel>(p => IdentityConfig);

            services.AddTransient<IClientApplicationRepository, ClientApplicationRepository>();
            services.AddTransient<ICustomUserRepository<TUser>, CustomUserRepository<TUser>>();

            services.AddTransient<ICustomUserManager<TUser>, CustomUserManager<TUser, TRole>>();
            services.AddTransient<ICustomRoleManager<TRole>, CustomRoleManager<TUser, TRole>>();
            services.AddTransient<IClientApplicationManager, ClientApplicationManager>();

            services.AddIdentity<TUser, TRole>(config =>
                {
                    config.SignIn.RequireConfirmedEmail = requireConfirmedEmail;
                })
                .AddEntityFrameworkStores<TContext>()
                .AddErrorDescriber<MultilanguageIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims

            return services;
        }
    }

}