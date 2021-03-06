﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace TDS.Server.Database.Extensions
{
    public static class CollectionEntryExtensions
    {

        public static async Task Reload(this CollectionEntry source)
        {
            if (source.CurrentValue is { })
            {
                foreach (var item in source.CurrentValue)
                    source.EntityEntry.Context.Entry(item).State = EntityState.Detached;
                source.CurrentValue = null;
            }
            source.IsLoaded = false;
            await source.LoadAsync().ConfigureAwait(false);
        }

    }
}
