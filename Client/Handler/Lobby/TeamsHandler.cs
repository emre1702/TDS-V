using System;
using System.Collections.Generic;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Lobby
{
    public class TeamsHandler
    {
        public List<SyncedTeamDataDto> LobbyTeams
        {
            get => _LobbyTeams;
            set
            {
                _LobbyTeams = value;
                if (_LobbyTeams != null)
                {
                    if (_LobbyTeams.Count == 1)
                    {
                        _modAPI.LocalPlayer.SetCanAttackFriendly(true);
                    }
                    else
                    {
                        _modAPI.LocalPlayer.SetCanAttackFriendly(false);
                    }

                }
            }
        }
        private readonly HashSet<IPlayer> _sameTeamPlayers = new HashSet<IPlayer>();
        public string CurrentTeamName { get; set; } = "Login/Register";
        public int AmountPlayersSameTeam => _sameTeamPlayers.Count;

        private bool _activated;
        private List<SyncedTeamDataDto> _LobbyTeams;

        private readonly IModAPI _modAPI;
        private readonly BrowserHandler _browserHandler;
        private readonly LobbyHandler _lobbyHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly CursorHandler _cursorHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly UtilsHandler _utilsHandler;
        private readonly BindsHandler _bindsHandler;
        private readonly Serializer _serializer;

        public TeamsHandler(IModAPI modAPI, BrowserHandler browserHandler, BindsHandler bindsHandler, LobbyHandler lobbyHandler, RemoteEventsSender remoteEventsSender, 
            CursorHandler cursorHandler, EventsHandler eventsHandler, UtilsHandler utilsHandler, Serializer serializer)
        {
            _modAPI = modAPI;
            _browserHandler = browserHandler;
            _lobbyHandler = lobbyHandler;
            _remoteEventsSender = remoteEventsSender;
            _cursorHandler = cursorHandler;
            _eventsHandler = eventsHandler;
            _utilsHandler = utilsHandler;
            _bindsHandler = bindsHandler;
            _serializer = serializer;

            eventsHandler.LoggedIn += EventsHandler_LoggedIn;

            modAPI.Event.Add(ToServerEvent.ChooseTeam, ChooseTeam);
            modAPI.Event.Add(ToClientEvent.ClearTeamPlayers, OnClearTeamPlayersMethod);
            modAPI.Event.Add(ToClientEvent.PlayerJoinedTeam, OnPlayerJoinedTeamMethod);
            modAPI.Event.Add(ToClientEvent.PlayerLeftTeam, OnPlayerLeftTeamMethod);
            modAPI.Event.Add(ToClientEvent.PlayerTeamChange, OnPlayerTeamChangeMethod);
            modAPI.Event.Add(ToClientEvent.SyncTeamChoiceMenuData, OnSyncTeamChoiceMenuDataMethod);
            modAPI.Event.Add(ToClientEvent.SyncTeamPlayers, OnSyncTeamPlayersMethod);
            modAPI.Event.Add(ToClientEvent.ToggleTeamChoiceMenu, OnToggleTeamChoiceMenuMethod);
        }

        public void AddSameTeam(IPlayer player)
        {
            //Todo: If the server crashes after a teammate dies, it could be because of the blip.
            // Then try removing the blip on death
            _sameTeamPlayers.Add(player);
            var prevBlipHandle = player.GetBlipFrom();
            if (prevBlipHandle != 0)
            {
                var blip = player.AddBlipFor();
                _modAPI.Ui.SetBlipAsFriendly(blip, true);
            }
        }

        public void RemoveSameTeam(IPlayer player)
        {
            _sameTeamPlayers.Remove(player);
            var prevBlipHandle = player.GetBlipFrom();
            if (_modAPI.Ui.DoesBlipExist(prevBlipHandle))
                _modAPI.Ui.RemoveBlip(ref prevBlipHandle);
        }

        public void ClearSameTeam()
        {
            foreach (var player in _sameTeamPlayers)
            {
                var blip = player.GetBlipFrom();
                if (_modAPI.Ui.DoesBlipExist(blip))
                    _modAPI.Ui.RemoveBlip(ref blip);
            }
            _sameTeamPlayers.Clear();
        }

        public bool IsInSameTeam(IPlayer player)
        {
            return _sameTeamPlayers.Contains(player);
        }

        public void ToggleOrderMode(Key _)
        {
            if (!_activated && _browserHandler.InInput)
                return;
            _activated = !_activated;
            _browserHandler.Angular.ToggleTeamOrderModus(_activated);
        }

        private void GiveOrder(Key key)
        {
            if (!_activated)
                return;
            if (!_lobbyHandler.InFightLobby)
                return;
            if (_browserHandler.InInput)
                return;

            TeamOrder order = GetTeamOrderByKey(key);
            _remoteEventsSender.Send(ToServerEvent.SendTeamOrder, (int)order);
            ToggleOrderMode(Key.Noname);
        }

        private TeamOrder GetTeamOrderByKey(Key key)
        {
            return (TeamOrder)(key - Key.Numpad1);
        }

        private void ChooseTeam(object[] args)
        {
            _browserHandler.Angular.ToggleTeamChoiceMenu(false);
            _cursorHandler.Visible = false;
            _eventsHandler.OnHideScoreboard();

            int index = Convert.ToInt32(args[0]);
            _remoteEventsSender.Send(ToServerEvent.ChooseTeam, index);
        }

        private void OnClearTeamPlayersMethod(object[] args)
        {
            ClearSameTeam();
        }

        private void OnPlayerJoinedTeamMethod(object[] args)
        {
            ushort handleValue = Convert.ToUInt16(args[0]);
            IPlayer player = _utilsHandler.GetPlayerByHandleValue(handleValue);
            AddSameTeam(player);
        }

        private void OnPlayerLeftTeamMethod(object[] args)
        {
            ushort handleValue = Convert.ToUInt16(args[0]);
            IPlayer player = _utilsHandler.GetPlayerByHandleValue(handleValue);
            RemoveSameTeam(player);
        }

        private void OnPlayerTeamChangeMethod(object[] args)
        {
            CurrentTeamName = (string)args[0];
            _eventsHandler.OnTeamChanged(CurrentTeamName);
        }

        private void EventsHandler_LoggedIn()
        {
            _bindsHandler.Add(Key.Numpad0, ToggleOrderMode);
            int i = 0;
            foreach (var orderobj in Enum.GetValues(typeof(TeamOrder)))
            {
                _bindsHandler.Add(Key.Numpad1 + (ushort)i, GiveOrder);
                ++i;
            }
        }

        private void OnSyncTeamChoiceMenuDataMethod(object[] args)
        {
            string teamsJson = (string)args[0];
            bool mixTeamsAfterRound = Convert.ToBoolean(args[1]);
            _browserHandler.Angular.SyncTeamChoiceMenuData(teamsJson, mixTeamsAfterRound);
            _eventsHandler.OnShowScoreboard();
            _cursorHandler.Visible = true;
        }

        private void OnSyncTeamPlayersMethod(object[] args)
        {
            ClearSameTeam();
            IEnumerable<int> listOfPlayerHandles = _serializer.FromServer<IEnumerable<int>>(args[0].ToString());
            foreach (var handle in listOfPlayerHandles)
            {
                IPlayer player = _modAPI.Pool.Players.GetAtHandle(handle);
                if (player != null)
                    AddSameTeam(player);
            }
        }

        private void OnToggleTeamChoiceMenuMethod(object[] args)
        {
            bool boolean = Convert.ToBoolean(args[0]);
            _cursorHandler.Visible = boolean;
            if (boolean)
                _eventsHandler.OnShowScoreboard();
            else
                _eventsHandler.OnHideScoreboard();
            _browserHandler.Angular.ToggleTeamChoiceMenu(boolean);
        }
    }
}
