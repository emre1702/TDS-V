using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Handler.GangSystem
{
    public class GangLevelsHandler
    {
        public Dictionary<byte, GangLevelSettings> Levels = new Dictionary<byte, GangLevelSettings>();

        public GangLevelsHandler(TDSDbContext dbContext)
        {
            LoadGangLevels(dbContext);
        }

        private void LoadGangLevels(TDSDbContext dbContext)
        {
            Levels = dbContext.GangLevelSettings.AsNoTracking().ToDictionary(l => l.Level, l => l);
        }
    }
}
