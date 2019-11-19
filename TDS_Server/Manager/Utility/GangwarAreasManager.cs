using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Instance.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Utility
{
    static class GangwarAreasManager
    {
        public static List<GangwarArea> GangwarAreas { get; set; } = new List<GangwarArea>();

        public static GangwarArea GetById(int id)
        {
            return GangwarAreas.FirstOrDefault(a => a.Entity.MapId == id);
        }

        public static void LoadGangwarAreas(TDSDbContext dbContext)
        {
            var entities = dbContext.GangwarAreas.Include(a => a.OwnerGang).ToList();
            foreach (var entity in entities)
            {
                GangwarAreas.Add(new GangwarArea(entity));
            }
        }
    }
}
