using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;

namespace TDS_Server.Entity.Gamemodes.ArmsRace
{
    partial class ArmsRace : Gamemode
    {
        #region Public Constructors

        public ArmsRace(
            IArena lobby, MapDto map, Serializer serializer, ISettingsHandler settingsHandler,
            LangHelper langHelper, InvitationsHandler invitationsHandler)
            : base(lobby, map, serializer, settingsHandler, langHelper, invitationsHandler)
        {
            LoadWeapons();
        }

        #endregion Public Constructors
    }
}
