using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Handler.GangSystem
{
    public class GangLevelsHandler
    {
        public Dictionary<byte, GangLevelSettings> Levels { get; private set; } = new Dictionary<byte, GangLevelSettings>();

        public byte HighestLevel => Levels.Keys.Max();

        public GangLevelsHandler(TDSDbContext dbContext) => LoadGangLevels(dbContext);

        private void LoadGangLevels(TDSDbContext dbContext)
        {
            Levels = dbContext.GangLevelSettings.AsNoTracking().ToDictionary(l => l.Level, l => l);
        }
    }
}
