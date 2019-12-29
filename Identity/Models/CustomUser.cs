using System;
using CustomFramework.BaseWebApi.Contracts;
using Microsoft.AspNetCore.Identity;

namespace CustomFramework.BaseWebApi.Identity.Models
{
    public class CustomUser : IdentityUser<int>
    {
        public Status Status { get; set; }

        public DateTime CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
        public DateTime? DeleteDateTime { get; set; }

        public int CreateUserId { get; set; }
        public int? UpdateUserId { get; set; }
        public int? DeleteUserId { get; set; }

        public DateTime LastSuccessfullLogin { get; set; }
        public DateTime LastLogOutDate { get; set; }
    }
}