using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;

namespace TDS_Server.Entity.Gamemodes.Gangwar
{
    public partial class Gangwar : Gamemode, IGangwar
    {
        private readonly TDSBlipHandler _tdsBlipHandler;
        private readonly TDSObjectHandler _tdsObjectHandler;
        private readonly TDSTextLabelHandler _tdsTextLabelHandler;

        #region Public Constructors

        public Gangwar(IArena lobby, MapDto map, Serializer serializer, ISettingsHandler settingsHandler, LangHelper langHelper,
            InvitationsHandler invitationsHandler, TDSBlipHandler tdsBlipHandler, TDSObjectHandler tdsObjectHandler, TDSTextLabelHandler tdsTextLabelHandler)
            : base(lobby, map, serializer, settingsHandler, langHelper, invitationsHandler)
        {
            _tdsBlipHandler = tdsBlipHandler;
            _tdsObjectHandler = tdsObjectHandler;
            _tdsTextLabelHandler = tdsTextLabelHandler;
        }

        #endregion Public Constructors
    }
}
