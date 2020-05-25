﻿using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Events;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Lobby
{
    public class LobbyChoiceHandler
    {
        #region Private Fields

        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly SettingsHandler _settingsHandler;

        #endregion Private Fields

        #region Public Constructors

        public LobbyChoiceHandler(IModAPI modAPI, RemoteEventsSender remoteEventsSender, SettingsHandler settingsHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            _settingsHandler = settingsHandler;

            modAPI.Event.Add(FromBrowserEvent.ChooseArenaToJoin, JoinArena);
            modAPI.Event.Add(FromBrowserEvent.ChooseCharCreatorToJoin, JoinCharCreator);
            modAPI.Event.Add(FromBrowserEvent.ChooseMapCreatorToJoin, JoinMapCreator);
        }

        #endregion Public Constructors

        #region Public Methods

        public void JoinArena(object[] args)
        {
            _remoteEventsSender.SendFromBrowser(ToServerEvent.JoinLobby, _settingsHandler.ArenaLobbyId);
        }

        public void JoinCharCreator(object[] args)
        {
            _remoteEventsSender.SendFromBrowser(ToServerEvent.JoinLobby, _settingsHandler.CharCreatorLobbyId);
        }

        public void JoinMapCreator(object[] args)
        {
            _remoteEventsSender.SendFromBrowser(ToServerEvent.JoinLobby, _settingsHandler.MapCreatorLobbyId);
        }

        #endregion Public Methods
    }
}
