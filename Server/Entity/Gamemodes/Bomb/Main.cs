using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;

namespace TDS_Server.Entity.Gamemodes.Bomb
{
    partial class Bomb : Gamemode, IBomb
    {
        #region Private Fields

        private readonly TDSMarkerHandler _tdsMarkerHandler;
        private readonly TDSObjectHandler _tdsObjectHandler;
        private readonly TDSBlipHandler _tdsBlipHandler;

        private readonly ITeam _counterTerroristTeam;
        private readonly ITeam _terroristTeam;
        private ITDSPlayer? _bombAtPlayer;

        #endregion Private Fields

        #region Public Constructors

        public Bomb(IArena arena, MapDto map, Serializer serializer, ISettingsHandler settingsHandler, LangHelper langHelper, InvitationsHandler invitationsHandler,
            TDSMarkerHandler tdsMarkerHandler, TDSObjectHandler tdsObjectHandler, TDSBlipHandler tdsBlipHandler)
            : base(arena, map, serializer, settingsHandler, langHelper, invitationsHandler)
        {
            _tdsMarkerHandler = tdsMarkerHandler;
            _tdsObjectHandler = tdsObjectHandler;
            _tdsBlipHandler = tdsBlipHandler;

            _terroristTeam = arena.Teams[2];
            _counterTerroristTeam = arena.Teams[1];
        }

        #endregion Public Constructors
    }
}
