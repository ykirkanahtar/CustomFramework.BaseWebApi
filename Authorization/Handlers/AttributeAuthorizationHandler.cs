﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CustomFramework.BaseWebApi.Authorization.Handlers
{
    public abstract class AttributeAuthorizationHandler<TRequirement, TAttribute> : AuthorizationHandler<TRequirement>
        where TRequirement : IAuthorizationRequirement
        where TAttribute : Attribute
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            var attributes = new List<TAttribute>();

            if (!((context.Resource as AuthorizationFilterContext)?.ActionDescriptor is ControllerActionDescriptor
                action)) return HandleRequirementAsync(context, requirement, attributes);

            attributes.AddRange(GetAttributes(action.ControllerTypeInfo.UnderlyingSystemType));
            attributes.AddRange(GetAttributes(action.MethodInfo));

            return HandleRequirementAsync(context, requirement, attributes);
        }

        protected abstract Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement,
            IEnumerable<TAttribute> attributes);

        private static IEnumerable<TAttribute> GetAttributes(ICustomAttributeProvider memberInfo)
        {
            return memberInfo.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>();
        }
    }
}