using CustomFramework.BaseWebApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomFramework.BaseWebApi.Data.ModelConfiguration
{
    public class BaseModelNonUserConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IBaseModelNonUser<TKey> where TKey : struct
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(p => p.Id).UseSqlServerIdentityColumn();

            builder.Property(p => p.CreateDateTime)
                .IsRequired();

            builder.Property(p => p.UpdateDateTime);

            builder.Property(p => p.DeleteDateTime);

            builder.Property(p => p.CreateClientApplicationId);
            builder.Property(p => p.UpdateClientApplicationId);
            builder.Property(p => p.DeleteClientApplicationId);

            builder.Property(p => p.Status)
                .IsRequired();

            builder.HasIndex(p => p.Status);
        }
    }
}