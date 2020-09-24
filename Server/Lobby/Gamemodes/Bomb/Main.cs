using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Handler;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.Gamemodes
{
    public partial class Bomb : Gamemode, IBomb
    {
        private readonly ITeam _counterTerroristTeam;
        private readonly ITeam _terroristTeam;
        private ITDSPlayer? _bombAtPlayer;

        public Bomb(ISettingsHandler settingsHandler, LangHelper langHelper, InvitationsHandler invitationsHandler)
            : base(settingsHandler, langHelper, invitationsHandler)
        {
            _terroristTeam = arena.Teams[2];
            _counterTerroristTeam = arena.Teams[1];
        }
    }
}
