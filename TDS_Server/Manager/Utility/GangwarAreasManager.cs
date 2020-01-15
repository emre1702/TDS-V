using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Maps;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Utility
{
    static class GangwarAreasManager
    {
        public static List<GangwarArea> GangwarAreas { get; set; } = new List<GangwarArea>();

        /// <summary>
        /// Returns the Gangwar area by Id / MapId.
        /// MapId is used as Id!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static GangwarArea? GetById(int id)
        {
            return GangwarAreas.FirstOrDefault(a => a.Entity?.MapId == id);
        }

        public static void LoadGangwarAreas(TDSDbContext dbContext)
        {
            var entities = dbContext.GangwarAreas.Include(a => a.Map).Include(a => a.OwnerGang).ToList();
            foreach (var entity in entities)
            {
                var map = MapsLoader.AllMaps.FirstOrDefault(m => m.Info.Type == Enums.EMapType.Gangwar && m.SyncedData.Id == entity.MapId);
                if (map is null)
                {
                    ErrorLogsManager.Log($"GangwarArea with Map {entity.Map.Name} ({entity.MapId}) has no map file in default maps folder!", Environment.StackTrace);
                    continue;
                }
                GangwarAreas.Add(new GangwarArea(entity, map));                
            }
        }
    }
}
