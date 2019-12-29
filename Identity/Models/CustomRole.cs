using CustomFramework.BaseWebApi.Contracts;
using Microsoft.AspNetCore.Identity;

namespace CustomFramework.BaseWebApi.Identity.Models
{
    public class CustomRole : IdentityRole<int>
    {
        public Status Status { get; set; }
    }
}