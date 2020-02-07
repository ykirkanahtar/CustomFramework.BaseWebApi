using CustomFramework.BaseWebApi.Identity.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CustomFramework.BaseWebApi.Data.ModelConfiguration;

namespace CustomFramework.BaseWebApi.Identity.Data.ModelConfigurations
{
    public class ClientApplicationModelConfiguration<T> : BaseModelNonUserConfiguration<T, int> where T : ClientApplication
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(u => u.ClientApplicationName)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(u => u.ClientApplicationCode)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(u => u.ClientApplicationPassword)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.SecurityStamp)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(p => p.ClientApplicationCode);
            builder.HasIndex(p => p.ClientApplicationName);
            builder.HasIndex(p => new { p.ClientApplicationCode, p.ClientApplicationPassword });

        }
    }

}