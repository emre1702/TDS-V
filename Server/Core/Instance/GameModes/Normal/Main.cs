using System.Linq;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server_DB.Entity;

namespace TDS_Server.Core.Instance.GameModes.Normal
{
    partial class Normal : GameMode
    {
        public Normal(Arena lobby, MapDto map) : base(lobby, map) { }

        public static void Init(TDSDbContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Select(w => w.Hash)
                .ToHashSet();
        }
    }
}
