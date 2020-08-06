using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Handler.GangSystem
{
    public class GangLevelsHandler
    {
        #region Public Fields

        public Dictionary<byte, GangLevelSettings> Levels = new Dictionary<byte, GangLevelSettings>();

        public byte HighestLevel => Levels.Keys.Max();

        #endregion Public Fields

        #region Public Constructors

        public GangLevelsHandler(TDSDbContext dbContext)
        {
            LoadGangLevels(dbContext);
        }

        #endregion Public Constructors

        #region Private Methods

        private void LoadGangLevels(TDSDbContext dbContext)
        {
            Levels = dbContext.GangLevelSettings.AsNoTracking().ToDictionary(l => l.Level, l => l);
        }

        #endregion Private Methods
    }
}
