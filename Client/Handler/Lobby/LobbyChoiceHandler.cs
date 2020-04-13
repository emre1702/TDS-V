﻿using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler;
using TDS_Client.Handler.Events;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Lobby
{
    public class LobbyChoiceHandler
    {
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly SettingsHandler _settingsHandler;

        public LobbyChoiceHandler(IModAPI modAPI, RemoteEventsSender remoteEventsSender, SettingsHandler settingsHandler)
        {
            _remoteEventsSender = remoteEventsSender;
            _settingsHandler = settingsHandler;

            modAPI.Event.Add(FromBrowserEvent.ChooseArenaToJoin, JoinArena);
            modAPI.Event.Add(FromBrowserEvent.ChooseMapCreatorToJoin, JoinMapCreator);
        }

        public void JoinArena(object[] args)
        {
            _remoteEventsSender.SendFromBrowser(ToServerEvent.JoinLobby, _settingsHandler.ArenaLobbyId);
        }

        public void JoinMapCreator(object[] args)
        {
            _remoteEventsSender.SendFromBrowser(ToServerEvent.JoinLobby, _settingsHandler.MapCreatorLobbyId);
        }
    }
}
