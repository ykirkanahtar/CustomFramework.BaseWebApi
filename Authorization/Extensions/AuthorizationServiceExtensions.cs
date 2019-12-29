using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace CustomFramework.BaseWebApi.Authorization.Extensions
{
    public static class AuthorizationServiceExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, IList<CustomAuthorizationPolicy> authorizationPolicies)
        {
            services.AddAuthorization(options =>
            {
                foreach (var authorizationPolicy in authorizationPolicies)
                {
                    options.AddPolicy(authorizationPolicy.Name, policyBuilder =>
                    {
                        foreach (var authorizationRequirement in authorizationPolicy.AuthorizationRequirements)
                        {
                            policyBuilder.Requirements.Add(authorizationRequirement);
                        }
                    });
                }
            });

            return services;
        }

    }
}