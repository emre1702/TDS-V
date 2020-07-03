using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.Gamemodes
{
    partial class ArmsRace : Gamemode
    {
        #region Public Constructors

        public ArmsRace(
            Arena lobby, MapDto map, IModAPI modAPI, Serializer serializer, ISettingsHandler settingsHandler,
            LangHelper langHelper, InvitationsHandler invitationsHandler)
            : base(lobby, map, modAPI, serializer, settingsHandler, langHelper, invitationsHandler)
        {
            LoadWeapons();
        }

        #endregion Public Constructors
    }
}
