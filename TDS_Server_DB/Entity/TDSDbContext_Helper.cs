using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TDS_Server_DB.Entity
{
    partial class TDSDbContext
    {
        public IEntityType GetDbEntityType(Type entityType)
        {
            return Model.FindEntityType(entityType);
        }

        public string GetTableName(Type entityType)
        {
            var entityDbType = Model.FindEntityType(entityType);
            if (entityDbType is null)
                return string.Empty;
            return GetTableName(entityDbType);
        }

        public string GetTableName(IEntityType entityDbType)
        {
            string schema = entityDbType.GetSchema();
            string table = entityDbType.GetTableName();

            return schema + "." + table;
        }

        public IProperty GetPropertyInfo(IEntityType entityDbType, string efCorePropertyName)
        {
            return entityDbType.FindProperty(efCorePropertyName);
        }

        public string GetPropertyName(IEntityType entityDbType, string efCorePropertyName)
        {
            var propertyInfo = entityDbType.FindProperty(efCorePropertyName);
            return propertyInfo?.GetColumnName();
        }
    }
}
