using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;

namespace TDS_Server.Entity.Gamemodes.TeamDeathmatch
{
    partial class Deathmatch : Gamemode, IDeathmatch
    {
        #region Public Constructors

        public Deathmatch(IArena lobby, MapDto map, Serializer serializer, ISettingsHandler settingsHandler, LangHelper langHelper, InvitationsHandler invitationsHandler)
            : base(lobby, map, serializer, settingsHandler, langHelper, invitationsHandler)
        {
        }

        #endregion Public Constructors
    }
}
