using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CustomFramework.BaseWebApi.Authorization.Extensions
{
    public static class JwtAuthenticationServiceExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, string validAudience, string validIssuer, string issuerSigningKey)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                    ValidateAudience = true,
                    ValidAudience = validAudience,
                    ValidateIssuer = true,
                    ValidIssuer = validIssuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(issuerSigningKey))
                    };

                    options.SaveToken = true;

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = ctx => Task.CompletedTask,
                            OnChallenge = context =>
                            {
                                context.Response.StatusCode = 401;
                                return Task.CompletedTask;
                            }
                    };
                });

            return services;
        }
    }
}