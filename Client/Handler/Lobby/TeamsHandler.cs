using System;
using System.Collections.Generic;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Events;
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
                        _modAPI.LocalPlayer.SetCanAttackFriendly(true, true);
                    }
                    else
                    {
                        _modAPI.LocalPlayer.SetCanAttackFriendly(false, false);
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

        public TeamsHandler(IModAPI modAPI, BrowserHandler browserHandler, BindsHandler bindsHandler, LobbyHandler lobbyHandler, RemoteEventsSender remoteEventsSender, 
            CursorHandler cursorHandler, EventsHandler eventsHandler)
        {
            _modAPI = modAPI;
            _browserHandler = browserHandler;
            _lobbyHandler = lobbyHandler;
            _remoteEventsSender = remoteEventsSender;
            _cursorHandler = cursorHandler;
            _eventsHandler = eventsHandler;

            bindsHandler.Add(Key.NumPad0, ToggleOrderMode);
            int i = 0;
            foreach (var orderobj in System.Enum.GetValues(typeof(TeamOrder)))
            {
                bindsHandler.Add(Key.NumPad1 + i, GiveOrder);
                ++i;
            }

            modAPI.Event.Add(ToServerEvent.ChooseTeam, ChooseTeam);
        }

        public void AddSameTeam(IPlayer player)
        {
            //Todo: If the server crashes after a teammate dies, it could be because of the blip.
            // Then try removing the blip on death
            _sameTeamPlayers.Add(player);
            var prevBlipHandle = player.GetBlipFrom();
            if (prevBlipHandle != null)
            {
                var blip = player.AddBlipFor();
                _modAPI.Ui.SetBlipAsFriendly(blip, true);
            }
        }

        public void RemoveSameTeam(IPlayer player)
        {
            _sameTeamPlayers.Remove(player);
            var prevBlipHandle = player.GetBlipFrom();
            if (prevBlipHandle != null && _modAPI.Ui.DoesBlipExist(prevBlipHandle))
                _modAPI.Ui.RemoveBlip(prevBlipHandle);
        }

        public void ClearSameTeam()
        {
            foreach (var player in _sameTeamPlayers)
            {
                var blip = player.GetBlipFrom();
                if (blip != null && _modAPI.Ui.DoesBlipExist(blip))
                    _modAPI.Ui.RemoveBlip(blip);
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
            ToggleOrderMode(Key.NoName);
        }

        private TeamOrder GetTeamOrderByKey(Key key)
        {
            return (TeamOrder)(key - Key.NumPad1);
        }

        private void ChooseTeam(object[] args)
        {
            _browserHandler.Angular.ToggleTeamChoiceMenu(false);
            _cursorHandler.Visible = false;
            _eventsHandler.OnLobbyJoinSelectedTeam();

            int index = Convert.ToInt32(args[0]);
            _remoteEventsSender.Send(ToServerEvent.ChooseTeam, index);
        }
    }
}
