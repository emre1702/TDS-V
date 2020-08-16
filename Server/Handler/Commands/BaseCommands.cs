using TDS_Server.Core.Manager.PlayerManager;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Sync;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Commands
{
    public partial class BaseCommands
    {
        #region Private Fields

        private readonly BansHandler _bansHandler;
        private readonly ChatHandler _chatHandler;
        private readonly CustomLobbyMenuSyncHandler _customLobbyMenuSyncHandler;
        private readonly DatabasePlayerHelper _databasePlayerHelper;
        private readonly InvitationsHandler _invitationsHandler;
        private readonly LangHelper _langHelper;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly Serializer _serializer;
        private readonly ISettingsHandler _settingsHandler;
        private readonly GangLevelsHandler _gangLevelsHandler;
        private readonly GangHousesHandler _gangHousesHandler;

        #endregion Private Fields

        #region Public Constructors

        public BaseCommands(CustomLobbyMenuSyncHandler customLobbyMenuSyncHandler, LobbiesHandler lobbiesHandler, ChatHandler chatHandler,
            ISettingsHandler settingsHandler, Serializer serializer, InvitationsHandler invitationsHandler, ILoggingHandler loggingHandler, LangHelper langHelper,
            DatabasePlayerHelper databasePlayerHelper, BansHandler bansHandler, GangLevelsHandler gangLevelsHandler,
            GangHousesHandler gangHousesHandler)
        {
            _customLobbyMenuSyncHandler = customLobbyMenuSyncHandler;
            _lobbiesHandler = lobbiesHandler;
            _chatHandler = chatHandler;
            _settingsHandler = settingsHandler;
            _serializer = serializer;
            _invitationsHandler = invitationsHandler;
            _loggingHandler = loggingHandler;
            _langHelper = langHelper;
            _databasePlayerHelper = databasePlayerHelper;
            _bansHandler = bansHandler;
            _gangLevelsHandler = gangLevelsHandler;
            _gangHousesHandler = gangHousesHandler;
        }

        #endregion Public Constructors
    }
}
