﻿using TDS_Server.Core.Manager.PlayerManager;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler.Account;
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
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly InvitationsHandler _invitationsHandler;
        private readonly LangHelper _langHelper;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ILoggingHandler _loggingHandler;
        private readonly IModAPI _modAPI;
        private readonly Serializer _serializer;
        private readonly ISettingsHandler _settingsHandler;

        #endregion Private Fields

        #region Public Constructors

        public BaseCommands(CustomLobbyMenuSyncHandler customLobbyMenuSyncHandler, LobbiesHandler lobbiesHandler, IModAPI modAPI, ChatHandler chatHandler,
            ISettingsHandler settingsHandler, Serializer serializer, InvitationsHandler invitationsHandler, ILoggingHandler loggingHandler, LangHelper langHelper,
            DatabasePlayerHelper databasePlayerHelper, BansHandler bansHandler, DataSyncHandler dataSyncHandler)
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
            _databasePlayerHelper = databasePlayerHelper;
            _bansHandler = bansHandler;
            _dataSyncHandler = dataSyncHandler;
        }

        #endregion Public Constructors
    }
}
