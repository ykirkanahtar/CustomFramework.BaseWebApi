using CustomFramework.BaseWebApi.Data.Enums;
using CustomFramework.Utils;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CustomFramework.BaseWebApi.Data.Utils
{

    public static class ModelBuilderExtensions
    {
        public static void ModelBuilderManager(this ModelBuilder modelBuilder, DatabaseProvider databaseProvider)
        {
            if (databaseProvider == DatabaseProvider.PostgreSql)
            {
                modelBuilder.SetModelToSnakeCase();
            }

            if (databaseProvider == DatabaseProvider.MsSql)
            {
                modelBuilder.HasDefaultSchema("dbo");
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        public static void BoolToInConverterForMySql(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(bool))
                    {
                        property.SetValueConverter(new BoolToIntConverter());
                    }
                }
            }
        }

        public static void SetModelToSnakeCase(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.Relational().TableName.ToSnakeCase();

                foreach (var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = property.Name.ToSnakeCase();
                }

                foreach (var key in entity.GetKeys())
                {
                    key.Relational().Name = key.Relational().Name.ToSnakeCase();
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.Relational().Name = key.Relational().Name.ToSnakeCase();
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.Relational().Name = index.Relational().Name.ToSnakeCase();
                }
            }
        }
    }
}