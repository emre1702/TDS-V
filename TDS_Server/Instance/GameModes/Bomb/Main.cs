using System.Linq;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.GameModes
{
    partial class Bomb : GameMode
    {
        private readonly Team _terroristTeam;
        private readonly Team _counterTerroristTeam;
        private TDSPlayer? _bombAtPlayer;

        public Bomb(Arena arena, MapDto map): base(arena, map)
        {
            _terroristTeam = arena.Teams[2];
            _counterTerroristTeam = arena.Teams[1];
        }

        public static void Init(TDSNewContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Select(w => w.Hash)
                .ToHashSet();
        }
    }
}
