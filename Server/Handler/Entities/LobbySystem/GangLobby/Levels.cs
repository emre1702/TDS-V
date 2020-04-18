using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class GangLobby
    {
        private Dictionary<byte, GangLevelSettings> _levels = new Dictionary<byte, GangLevelSettings>();

        private void LoadGangLevels()
        {
            _levels = ExecuteForDB(dbContext =>
            {
                return dbContext.GangLevelSettings.ToDictionary(l => l.Level, l => l);
            }).Result;
        }
    }
}
