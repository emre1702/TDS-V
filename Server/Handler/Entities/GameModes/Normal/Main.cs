using System.Linq;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Database.Entity;

namespace TDS_Server.Handler.Entities.GameModes.Normal
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
