using System.Collections.Generic;
using System.Linq;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.Lobby;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.GameModes
{
    partial class Sniper : GameMode
    {
        public Sniper(Arena lobby, MapDto map) : base(lobby, map) { }

        public static void Init(TDSNewContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Where(w => w.Type == TDS_Common.Enum.EWeaponType.SniperRifle)
                .Select(w => w.Hash)
                .ToHashSet();
        }
    }
}
