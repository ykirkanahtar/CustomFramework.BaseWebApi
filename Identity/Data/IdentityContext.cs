using CustomFramework.BaseWebApi.Identity.Data.ModelConfigurations;
using CustomFramework.BaseWebApi.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CustomFramework.BaseWebApi.Identity.Data
{
    public class IdentityContext<TUser, TRole> : IdentityDbContext<TUser, TRole, int>
        where TUser : CustomUser
        where TRole : CustomRole
    {

        public IdentityContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TUser>().ToTable("Users");
            builder.Entity<TRole>().ToTable("Roles");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");


            builder.ApplyConfiguration(new ClientApplicationModelConfiguration<ClientApplication>());
        }

        public virtual DbSet<ClientApplication> ClientApplications { get; set; }
    }
}