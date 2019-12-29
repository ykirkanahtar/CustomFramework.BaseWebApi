using System;
using CustomFramework.BaseWebApi.Authorization.Enums;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace CustomFramework.BaseWebApi.Authorization.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute : AuthorizeAttribute
    {
        public string Entity { get; set; }

        public Crud? Crud { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public PermissionAttribute(string claimType) : base("Permission")
        {
            ClaimType = claimType;
        }

        [JsonConstructor]
        public PermissionAttribute(string entity, Crud crud) : base("Permission")
        {
            Entity = entity;
            Crud = crud;
        }

        public PermissionAttribute(string claimType, string claimValue) : base("Permission")
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }
    }
}