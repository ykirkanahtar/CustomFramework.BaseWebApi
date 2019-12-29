using CustomFramework.BaseWebApi.Data.ModelConfiguration;
using CustomFramework.BaseWebApi.LogProvider.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomFramework.BaseWebApi.LogProvider.Data.ModelConfigurations
{
    public class LogModelConfiguration<T> : BaseModelNonUserConfiguration<T, long> where T : Log
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.RequestBody).IsRequired().HasMaxLength(2500);
            builder.Property(p => p.RequestMethod).HasMaxLength(20);
            builder.Property(p => p.RequestUrl).IsRequired().HasMaxLength(250);
            builder.Property(p => p.RequestTime).IsRequired();
            builder.Property(p => p.ResponseBody).IsRequired().HasMaxLength(5000);
            builder.Property(p => p.ResponseTime).IsRequired();
            builder.Property(p => p.LoggedUserId);
        }
    }
}