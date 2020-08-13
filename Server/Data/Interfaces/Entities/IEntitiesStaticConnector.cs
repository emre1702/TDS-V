using System.Collections.Generic;
using TDS_Server.Data.Enums;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Data.Interfaces.Entities
{
    public interface IEntitiesStaticConnector
    {
        HashSet<LobbyWeapons> GetAllowedWeapons(MapType type);
        void InitGamemodes(TDSDbContext dbContext);
    }
}
