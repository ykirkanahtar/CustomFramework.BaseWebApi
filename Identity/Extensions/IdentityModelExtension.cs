using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CustomFramework.EmailProvider;
using CustomFramework.BaseWebApi.Identity.Business;
using CustomFramework.BaseWebApi.Identity.Data.Repositories;
using CustomFramework.BaseWebApi.Identity.Handlers;
using CustomFramework.BaseWebApi.Identity.Models;
using CustomFramework.BaseWebApi.Identity.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using CustomFramework.BaseWebApi.Authorization.Utils;
using CustomFramework.BaseWebApi.Authorization.Attributes;

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

            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSingleton<IToken, Token>(p => IdentityConfig.Token);
            services.AddSingleton<IEmailConfig, EmailConfig>(p => IdentityConfig.EmailConfig);
            services.AddSingleton<IIdentityModel, IdentityModel>(p => IdentityConfig);

            services.AddTransient<IClientApplicationRepository, ClientApplicationRepository>();
            services.AddTransient<ICustomUserRepository<TUser>, CustomUserRepository<TUser>>();

            services.AddTransient<ICustomUserManager<TUser>, CustomUserManager<TUser, TRole>>();
            services.AddTransient<ICustomRoleManager<TRole>, CustomRoleManager<TUser, TRole>>();
            services.AddTransient<IClientApplicationManager, ClientApplicationManager>();
            services.AddTransient<IPermissionManager, PermissionManager<TUser, TRole>>();

            services.AddIdentity<TUser, TRole>(config =>
                {
                    config.SignIn.RequireConfirmedEmail = requireConfirmedEmail;
                    config.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
                    config.Tokens.ChangeEmailTokenProvider = TokenOptions.DefaultProvider;
                    config.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;
                })
                .AddEntityFrameworkStores<TContext>()
                .AddErrorDescriber<MultilanguageIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = token.Issuer,
                        ValidAudience = token.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token.Key)),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission", p =>
                    p.Requirements.Add(new PermissionAuthorizationRequirement()));
            });

            return services;
        }
    }

}