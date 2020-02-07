using CustomFramework.BaseWebApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomFramework.BaseWebApi.Data.ModelConfiguration
{
    public class BaseModelConfiguration<TEntity, TKey> : BaseModelNonUserConfiguration<TEntity,TKey>, IEntityTypeConfiguration<TEntity>
        where TEntity : class, IBaseModel<TKey> where TKey : struct
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.CreateUserId).IsRequired();

            builder.Property(p => p.UpdateUserId);

            builder.Property(p => p.DeleteUserId);

            builder.HasIndex(p => p.CreateUserId);
        }
    }
}