using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace TDS_Server.Database.Entity
{
    partial class TDSDbContext
    {
        #region Public Methods

        public IEntityType GetDbEntityType(Type entityType)
        {
            return Model.FindEntityType(entityType);
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

            if (schema is null)
                return table;

            return schema + "." + table;
        }

        #endregion Public Methods
    }
}
