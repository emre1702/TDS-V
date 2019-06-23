using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TDS_Server_DB
{
    static class ModelBuilderExtend
    {
        public static bool IsSignedInteger(this Type type)
           => type == typeof(int)
              || type == typeof(long)
              || type == typeof(short)
              || type == typeof(sbyte);

        public static void Seed<T>(this ModelBuilder modelBuilder, IEnumerable<T> data) where T : class
        {
            var entnty = modelBuilder.Entity<T>();

            var pk = entnty.Metadata
                .GetProperties()
                .FirstOrDefault(property =>
                   property.IsPrimaryKey()
                );
            if (pk != null && entnty.Property(pk.Name).Metadata.ValueGenerated == ValueGenerated.OnAdd)
            {
                entnty.Property(pk.Name).ValueGeneratedNever();
                entnty.HasData(data);
                entnty.Property(pk.Name).ValueGeneratedOnAdd();
            }
            else
            {
                entnty.HasData(data);
            }
        }
    }
}
