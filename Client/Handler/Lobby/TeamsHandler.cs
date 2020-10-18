using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Client.Data.Enums;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Lobby
{
    public class TeamsHandler : ServiceBase
    {
        #region Public Fields

        public readonly HashSet<ITDSPlayer> SameTeamPlayers = new HashSet<ITDSPlayer>();

        #endregion Public Fields

        #region Private Fields

        private readonly BindsHandler _bindsHandler;

        private readonly BrowserHandler _browserHandler;

        private readonly CursorHandler _cursorHandler;

        private readonly EventsHandler _eventsHandler;

        private readonly LobbyHandler _lobbyHandler;

        private readonly RemoteEventsSender _remoteEventsSender;

        private readonly UtilsHandler _utilsHandler;

        private bool _activated;

        private List<SyncedTeamDataDto> _LobbyTeams;

        #endregion Private Fields

        #region Public Constructors

        public TeamsHandler(LoggingHandler loggingHandler, BrowserHandler browserHandler, BindsHandler bindsHandler, LobbyHandler lobbyHandler,
            RemoteEventsSender remoteEventsSender, CursorHandler cursorHandler, EventsHandler eventsHandler, UtilsHandler utilsHandler)
            : base(loggingHandler)
        {
            _browserHandler = browserHandler;
            _lobbyHandler = lobbyHandler;
            _remoteEventsSender = remoteEventsSender;
            _cursorHandler = cursorHandler;
            _eventsHandler = eventsHandler;
            _utilsHandler = utilsHandler;
            _bindsHandler = bindsHandler;

            eventsHandler.LoggedIn += EventsHandler_LoggedIn;

            RAGE.Events.Add(ToServerEvent.ChooseTeam, ChooseTeam);
            RAGE.Events.Add(ToClientEvent.ClearTeamPlayers, OnClearTeamPlayersMethod);
            RAGE.Events.Add(ToClientEvent.PlayerJoinedTeam, OnPlayerJoinedTeamMethod);
            RAGE.Events.Add(ToClientEvent.PlayerLeftTeam, OnPlayerLeftTeamMethod);
            RAGE.Events.Add(ToClientEvent.PlayerTeamChange, OnPlayerTeamChangeMethod);
            RAGE.Events.Add(ToClientEvent.SyncTeamChoiceMenuData, OnSyncTeamChoiceMenuDataMethod);
            RAGE.Events.Add(ToClientEvent.SyncTeamPlayers, OnSyncTeamPlayersMethod);
            RAGE.Events.Add(ToClientEvent.ToggleTeamChoiceMenu, OnToggleTeamChoiceMenuMethod);
        }

        #endregion Public Constructors

        #region Public Properties

        public int AmountPlayersSameTeam => SameTeamPlayers.Count;

        public string CurrentTeamName { get; set; } = "Login/Register";

        public List<SyncedTeamDataDto> LobbyTeams
        {
            get => _LobbyTeams;
            set
            {
                _LobbyTeams = value;
                if (_LobbyTeams != null)
                {
                    if (_LobbyTeams.Count(t => !t.IsSpectator) == 1)
                    {
                        RAGE.Elements.Player.LocalPlayer.SetCanAttackFriendly(true, false);
                    }
                    else
                    {
                        RAGE.Elements.Player.LocalPlayer.SetCanAttackFriendly(false, false);
                    }
                }
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void AddSameTeam(ITDSPlayer player)
        {
            try
            {
                Logging.LogInfo("", "TeamsHandler.AddSameTeam");
                SameTeamPlayers.Add(player);
                var prevBlipHandle = player.GetBlipFrom();
                if (prevBlipHandle != 0)
                {
                    var blip = player.AddBlipFor();
                    RAGE.Game.Ui.SetBlipAsFriendly(blip, true);
                }
                Logging.LogInfo("", "TeamsHandler.AddSameTeam", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void ClearSameTeam()
        {
            try
            {
                Logging.LogInfo("", "TeamsHandler.ClearSameTeam");
                foreach (var player in SameTeamPlayers)
                {
                    var blip = player.GetBlipFrom();
                    if (RAGE.Game.Ui.DoesBlipExist(blip))
                        RAGE.Game.Ui.RemoveBlip(ref blip);
                }
                SameTeamPlayers.Clear();
                Logging.LogInfo("", "TeamsHandler.ClearSameTeam", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public bool IsInSameTeam(ITDSPlayer player)
        {
            return SameTeamPlayers.Contains(player);
        }

        public void RemoveSameTeam(ITDSPlayer player)
        {
            Logging.LogInfo("", "TeamsHandler.RemoveSameTeam");
            SameTeamPlayers.Remove(player);
            var prevBlipHandle = player.GetBlipFrom();
            if (RAGE.Game.Ui.DoesBlipExist(prevBlipHandle))
                RAGE.Game.Ui.RemoveBlip(ref prevBlipHandle);
            Logging.LogInfo("", "TeamsHandler.RemoveSameTeam", true);
        }

        public void RemoveSameTeam(ushort remoteId)
        {
            Logging.LogInfo(remoteId.ToString(), "TeamsHandler.RemoveSameTeam");

            var player = SameTeamPlayers.FirstOrDefault(p => p.RemoteId == remoteId);
            if (player is null)
                return;
            RemoveSameTeam(player);

            Logging.LogInfo(remoteId.ToString(), "TeamsHandler.RemoveSameTeam", true);
        }

        public void ToggleOrderMode(Key _)
        {
            if (!_activated && _browserHandler.InInput)
                return;
            _activated = !_activated;
            _browserHandler.Angular.ToggleTeamOrderModus(_activated);
        }

        #endregion Public Methods

        #region Private Methods

        private void ChooseTeam(object[] args)
        {
            try
            {
                Logging.LogInfo("", "TeamsHandler.ChooseTeam");
                _browserHandler.Angular.ToggleTeamChoiceMenu(false);
                _cursorHandler.Visible = false;
                _eventsHandler.OnHideScoreboard();

                int index = Convert.ToInt32(args[0]);
                _remoteEventsSender.Send(ToServerEvent.ChooseTeam, index);
                Logging.LogInfo("", "TeamsHandler.ChooseTeam", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
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

        private TeamOrder GetTeamOrderByKey(Key key)
        {
            return (TeamOrder)(key - Key.Numpad1);
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

        private void OnClearTeamPlayersMethod(object[] args)
        {
            ClearSameTeam();
        }

        private void OnPlayerJoinedTeamMethod(object[] args)
        {
            try
            {
                Logging.LogInfo("", "TeamsHandler.OnPlayerJoinedTeamMethod");
                ushort handleValue = Convert.ToUInt16(args[0]);
                ITDSPlayer player = _utilsHandler.GetPlayerByHandleValue(handleValue);
                AddSameTeam(player);
                Logging.LogInfo("", "TeamsHandler.OnPlayerJoinedTeamMethod", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnPlayerLeftTeamMethod(object[] args)
        {
            try
            {
                Logging.LogInfo("", "TeamsHandler.OnPlayerLeftTeamMethod");
                ushort handleValue = Convert.ToUInt16(args[0]);
                ITDSPlayer player = _utilsHandler.GetPlayerByHandleValue(handleValue);
                if (player is null)
                    RemoveSameTeam(handleValue);
                else
                    RemoveSameTeam(player);
                Logging.LogInfo("", "TeamsHandler.OnPlayerLeftTeamMethod", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnPlayerTeamChangeMethod(object[] args)
        {
            try
            {
                Logging.LogInfo("", "TeamsHandler.OnPlayerTeamChangeMethod");
                CurrentTeamName = (string)args[0];
                _eventsHandler.OnTeamChanged(CurrentTeamName);
                Logging.LogInfo("", "TeamsHandler.OnPlayerTeamChangeMethod", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnSyncTeamChoiceMenuDataMethod(object[] args)
        {
            try
            {
                Logging.LogInfo("", "TeamsHandler.OnSyncTeamChoiceMenuDataMethod");
                string teamsJson = (string)args[0];
                bool mixTeamsAfterRound = Convert.ToBoolean(args[1]);
                _browserHandler.Angular.SyncTeamChoiceMenuData(teamsJson, mixTeamsAfterRound);
                _eventsHandler.OnShowScoreboard();
                _cursorHandler.Visible = true;
                Logging.LogInfo("", "TeamsHandler.OnSyncTeamChoiceMenuDataMethod", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnSyncTeamPlayersMethod(object[] args)
        {
            try
            {
                Logging.LogInfo("", "TeamsHandler.OnSyncTeamPlayersMethod");
                ClearSameTeam();
                var listOfPlayers = _utilsHandler.GetTriggeredPlayersList((string)args[0]);
                foreach (var player in listOfPlayers)
                {
                    AddSameTeam(player);
                }
                Logging.LogInfo("", "TeamsHandler.OnSyncTeamPlayersMethod", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnToggleTeamChoiceMenuMethod(object[] args)
        {
            try
            {
                Logging.LogInfo("", "TeamsHandler.OnToggleTeamChoiceMenuMethod");
                bool boolean = Convert.ToBoolean(args[0]);
                _cursorHandler.Visible = boolean;
                if (boolean)
                    _eventsHandler.OnShowScoreboard();
                else
                    _eventsHandler.OnHideScoreboard();
                _browserHandler.Angular.ToggleTeamChoiceMenu(boolean);
                Logging.LogInfo("", "TeamsHandler.OnToggleTeamChoiceMenuMethod", true);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        #endregion Private Methods
    }
}
