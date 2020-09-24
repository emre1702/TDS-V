using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.Gamemodes
{
    public partial class Deathmatch : Gamemode, IDeathmatch
    {
        public Deathmatch(ISettingsHandler settingsHandler, LangHelper langHelper, InvitationsHandler invitationsHandler)
            : base(settingsHandler, langHelper, invitationsHandler)
        {
        }
    }
}
