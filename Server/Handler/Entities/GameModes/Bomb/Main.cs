using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.Gamemodes
{
    partial class Bomb : Gamemode
    {
        private readonly ITeam _counterTerroristTeam;
        private readonly ITeam _terroristTeam;
        private ITDSPlayer? _bombAtPlayer;

        public Bomb(Arena arena, MapDto map, Serializer serializer, ISettingsHandler settingsHandler, LangHelper langHelper, InvitationsHandler invitationsHandler)
            : base(arena, map, serializer, settingsHandler, langHelper, invitationsHandler)
        {
            _terroristTeam = arena.Teams[2];
            _counterTerroristTeam = arena.Teams[1];
        }
    }
}
