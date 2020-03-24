using TDS_Server.Core.Manager.PlayerManager;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler.Helper;
using TDS_Server.Handler.Player;
using TDS_Server.Handler.Sync;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Commands
{
    public partial class BaseCommands
    {
        private readonly CustomLobbyMenuSyncHandler _customLobbyMenuSyncHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly IModAPI _modAPI;
        private readonly ChatHandler _chatHandler;
        private readonly ISettingsHandler _settingsHandler;
        private readonly Serializer _serializer;
        private readonly InvitationsHandler _invitationsHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly LangHelper _langHelper;
        private readonly TDSPlayerHandler _tdsPlayerHandler;
        private readonly DatabasePlayerHelper _databasePlayerHelper;

        public BaseCommands(CustomLobbyMenuSyncHandler customLobbyMenuSyncHandler, LobbiesHandler lobbiesHandler, IModAPI modAPI, ChatHandler chatHandler,
            ISettingsHandler settingsHandler, Serializer serializer, InvitationsHandler invitationsHandler, ILoggingHandler loggingHandler, LangHelper langHelper,
            TDSPlayerHandler tdsPlayerHandler, DatabasePlayerHelper databasePlayerHelper)
        {
            _customLobbyMenuSyncHandler = customLobbyMenuSyncHandler;
            _lobbiesHandler = lobbiesHandler;
            _modAPI = modAPI;
            _chatHandler = chatHandler;
            _settingsHandler = settingsHandler;
            _serializer = serializer;
            _invitationsHandler = invitationsHandler;
            _loggingHandler = loggingHandler;
            _langHelper = langHelper;
            _tdsPlayerHandler = tdsPlayerHandler;
            _databasePlayerHelper = databasePlayerHelper;
        }
    }
}
