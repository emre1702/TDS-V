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
        private readonly BansHandler _bansHandler;
        private readonly ChatHandler _chatHandler;
        private readonly CustomLobbyMenuSyncHandler _customLobbyMenuSyncHandler;
        private readonly DatabasePlayerHelper _databasePlayerHelper;
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly InvitationsHandler _invitationsHandler;
        private readonly LangHelper _langHelper;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ISettingsHandler _settingsHandler;
        private readonly GangLevelsHandler _gangLevelsHandler;
        private readonly GangHousesHandler _gangHousesHandler;

        public BaseCommands(CustomLobbyMenuSyncHandler customLobbyMenuSyncHandler, LobbiesHandler lobbiesHandler, ChatHandler chatHandler,
            ISettingsHandler settingsHandler, InvitationsHandler invitationsHandler, ILoggingHandler loggingHandler, LangHelper langHelper,
            DatabasePlayerHelper databasePlayerHelper, BansHandler bansHandler, DataSyncHandler dataSyncHandler, GangLevelsHandler gangLevelsHandler,
            GangHousesHandler gangHousesHandler)
        {
            _customLobbyMenuSyncHandler = customLobbyMenuSyncHandler;
            _lobbiesHandler = lobbiesHandler;
            _chatHandler = chatHandler;
            _settingsHandler = settingsHandler;

            _invitationsHandler = invitationsHandler;
            _loggingHandler = loggingHandler;
            _langHelper = langHelper;
            _databasePlayerHelper = databasePlayerHelper;
            _bansHandler = bansHandler;
            _dataSyncHandler = dataSyncHandler;
            _gangLevelsHandler = gangLevelsHandler;
            _gangHousesHandler = gangHousesHandler;
        }
    }
}
