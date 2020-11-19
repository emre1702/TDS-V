using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace TDS.Server.Database.Entity
{
    public static class TDSDbContextExtensions
    {
        public static IEntityType GetDbEntityType(this TDSDbContext dbContext, Type entityType)
        {
            return dbContext.Model.FindEntityType(entityType);
        }

        public static IProperty GetPropertyInfo(this TDSDbContext dbContext, IEntityType entityDbType, string efCorePropertyName)
        {
            return entityDbType.FindProperty(efCorePropertyName);
        }

        public static string GetTableName(this TDSDbContext dbContext, Type entityType)
        {
            var entityDbType = dbContext.Model.FindEntityType(entityType);
            if (entityDbType is null)
                return string.Empty;
            return dbContext.GetTableName(entityDbType);
        }

        public static string GetTableName(this TDSDbContext dbContext, IEntityType entityDbType)
        {
            string schema = entityDbType.GetSchema();
            string table = entityDbType.GetTableName();

            if (schema is null)
                return table;

            return schema + "." + table;
        }

    }
}
