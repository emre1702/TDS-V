using System.Linq;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Instance.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.GameModes
{
    partial class Gangwar : GameMode
    {
        private readonly GangwarArea _gangwarArea;

        public Gangwar(Arena lobby, MapDto map) : base(lobby, map) 
        { 
            _gangwarArea = lobby.GangwarArea!;    
        }

        public static void Init(TDSDbContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Select(w => w.Hash)
                .ToHashSet();

            
        }
    }
}
