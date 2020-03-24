using System.Linq;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Entities.GameModes.Bomb
{
    partial class Bomb : GameMode
    {
        private readonly ITeam _terroristTeam;
        private readonly ITeam _counterTerroristTeam;
        private ITDSPlayer? _bombAtPlayer;

        public Bomb(Arena arena, MapDto map, IModAPI modAPI, Serializer serializer, SettingsHandler settingsHandler, LangHelper langHelper, InvitationsHandler invitationsHandler)
            : base(arena, map, modAPI, serializer, settingsHandler, langHelper, invitationsHandler)
        {
            _terroristTeam = arena.Teams[2];
            _counterTerroristTeam = arena.Teams[1];
        }

        public static void Init(TDSDbContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Select(w => w.Hash)
                .ToHashSet();
        }
    }
}
