using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Client.Data.Enums;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Events;
using TDS_Shared.Core;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;
using static RAGE.Events;
using static RAGE.NUI.UIResText;

namespace TDS_Client.Handler.Lobby
{
    public class RoundInfosHandler : ServiceBase
    {
        private readonly DxHandler _dxHandler;

        private readonly Serializer _serializer;

        private readonly SettingsHandler _settingsHandler;

        private readonly TeamsHandler _teamsHandler;

        private readonly TimerHandler _timerHandler;

        private bool _refreshOnTick;

        private int _startedMs;

        private DxTextRectangle[] _teamDisplays;

        private DxTextRectangle _timeDisplay;

        public RoundInfosHandler(LoggingHandler loggingHandler, TeamsHandler teamsHandler, TimerHandler timerHandler, DxHandler dxHandler,
            SettingsHandler settingsHandler, EventsHandler eventsHandler, Serializer serializer)
            : base(loggingHandler)
        {
            _teamsHandler = teamsHandler;
            _timerHandler = timerHandler;
            _dxHandler = dxHandler;
            _settingsHandler = settingsHandler;
            _serializer = serializer;

            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
            eventsHandler.PlayerDied += EventsHandler_PlayerDied;

            eventsHandler.CountdownStarted += EventsHandler_CountdownStarted;
            eventsHandler.RoundStarted += EventsHandler_RoundStarted;
            eventsHandler.RoundEnded += EventsHandler_RoundEnded;

            Add(ToClientEvent.AmountInFightSync, OnAmountInFightSyncMethod);
            Add(ToClientEvent.StopRoundStats, OnStopRoundStatsMethod);
        }

        public bool RefreshOnTick
        {
            get => _refreshOnTick;
            set
            {
                if (_refreshOnTick == value)
                    return;
                _refreshOnTick = value;
                if (value)
                    Tick += RefreshTime;
                else
                    Tick -= RefreshTime;
            }
        }

        public void OnePlayerDied(int teamindex)
        {
            try
            {
                if (teamindex < 0 || teamindex >= _teamsHandler.LobbyTeams.Count)
                    return;
                --_teamsHandler.LobbyTeams[teamindex].AmountPlayers.AmountAlive;
                RefreshTeamText(teamindex);
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void RefreshAllTeamTexts()
        {
            foreach (var team in _teamsHandler.LobbyTeams)
            {
                if (team.IsSpectator)
                    continue;
                RefreshTeamText(team.Index);
            }
        }

        public void RefreshTeamText(int index)
        {
            if (_teamDisplays == null)
                return;
            SyncedTeamDataDto team = _teamsHandler.LobbyTeams[index];
            _teamDisplays[index - 1].SetText(team.Name + "\n" + team.AmountPlayers.AmountAlive + "/" + team.AmountPlayers.Amount);
        }

        public void RefreshTime(List<TickNametagData> _)
        {
            var elapsedMs = (_timerHandler.ElapsedMs - _startedMs);
            double timems = 0;
            if (_settingsHandler.RoundTime * 1000 > elapsedMs)
                timems = _settingsHandler.RoundTime * 1000 - (_timerHandler.ElapsedMs - _startedMs);
            _timeDisplay?.SetText(TimeSpan.FromMilliseconds(timems).ToString(@"mm\:ss"));
        }

        public void SetRoundTimeLeft(int lefttimems)
        {
            _startedMs = _timerHandler.ElapsedMs - (_settingsHandler.RoundTime * 1000 - lefttimems);
        }

        public void Start(int elapsedMsSinceRoundStart)
        {
            try
            {
                _startedMs = _timerHandler.ElapsedMs - elapsedMsSinceRoundStart;

                if (_teamDisplays != null)
                    Stop();

                CreateTimeDisplay();
                CreateTeamsDisplays();
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void Stop()
        {
            try
            {
                RefreshOnTick = false;
                if (_timeDisplay != null)
                    _timeDisplay.Activated = false;
                if (_teamDisplays != null)
                {
                    foreach (var display in _teamDisplays)
                    {
                        display.Remove();
                    }
                    _teamDisplays = null;
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void StopDeathmatchInfo()
        {
            // stop damage, assists and kills display here
        }

        private void CreateTeamsDisplays()
        {
            var teams = _teamsHandler.LobbyTeams;
            int showamountleft = (int)Math.Ceiling((teams.Count - 1) / 2d);
            int showamountright = teams.Count - showamountleft - 1;
            _teamDisplays = new DxTextRectangle[teams.Count - 1];
            for (int i = 0; i < showamountleft; ++i)
            {
                float x = 0.5f - 0.06f * 0.5f - 0.13f * i - 0.13f * 0.5f;
                var team = teams[i + 1];
                _teamDisplays[i] = new DxTextRectangle(_dxHandler, _timerHandler, team.Name + "\n" + team.AmountPlayers.AmountAlive + "/" + team.AmountPlayers.Amount, x, 0, 0.13f, 0.06f,
                    Color.White, Color.FromArgb(187, team.Color.R, team.Color.G, team.Color.B), 0.41f, alignment: Alignment.Centered, alignmentY: AlignmentY.Top, amountLines: 2);
            }
            for (int j = 0; j < showamountright; ++j)
            {
                float x = 0.5f + 0.06f * 0.5f + 0.13f * j + 0.13f * 0.5f;
                int i = j + showamountleft;
                var team = teams[i + 1];
                _teamDisplays[i] = new DxTextRectangle(_dxHandler, _timerHandler, team.Name + "\n" + team.AmountPlayers.AmountAlive + "/" + team.AmountPlayers.Amount, x, 0, 0.13f, 0.06f,
                    Color.White, Color.FromArgb(187, team.Color.R, team.Color.G, team.Color.B), 0.41f, alignment: Alignment.Centered, alignmentY: AlignmentY.Top, amountLines: 2);
            }
        }

        private void CreateTimeDisplay()
        {
            if (_timeDisplay == null)
                _timeDisplay = new DxTextRectangle(_dxHandler, _timerHandler, "00:00", 0.5f, 0f, 0.06f, 0.05f,
                            Color.White, Color.FromArgb(180, 20, 20, 20), textScale: 0.5f, alignment: Alignment.Centered, alignmentY: AlignmentY.Top);
            _timeDisplay.Activated = true;
            RefreshOnTick = true;
        }

        private void EventsHandler_CountdownStarted(bool isSpectator)
        {
            if (!_settingsHandler.PlayerSettings.WindowsNotifications)
                return;
            if (isSpectator)
                return;
            RAGE.Ui.Windows.Notify(_settingsHandler.Language.ROUND_INFOS, _settingsHandler.Language.COUNTDOWN_STARTED_NOTIFICATION, "TDS-V", 4000);
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            Stop();
        }

        private void EventsHandler_PlayerDied(ITDSPlayer player, int teamIndex, bool willRespawn)
        {
            if (willRespawn)
                return;
            OnePlayerDied(teamIndex);
        }

        private void EventsHandler_RoundEnded(bool isSpectator)
        {
            Stop();
            if (!_settingsHandler.PlayerSettings.WindowsNotifications)
                return;
            if (isSpectator)
                return;
            RAGE.Ui.Windows.Notify(_settingsHandler.Language.ROUND_INFOS, _settingsHandler.Language.ROUND_ENDED_NOTIFICATION, "TDS-V", 4000);
        }

        private void EventsHandler_RoundStarted(bool isSpectator)
        {
            if (!_settingsHandler.PlayerSettings.WindowsNotifications)
                return;
            if (isSpectator)
                return;
            RAGE.Ui.Windows.Notify(_settingsHandler.Language.ROUND_INFOS, _settingsHandler.Language.ROUND_STARTED_NOTIFICATION, "TDS-V", 4000);
        }

        private void OnAmountInFightSyncMethod(object[] args)
        {
            try
            {
                SyncedTeamPlayerAmountDto[] list = _serializer.FromServer<SyncedTeamPlayerAmountDto[]>((string)args[0]);
                foreach (var team in _teamsHandler.LobbyTeams)
                {
                    if (!team.IsSpectator)
                        team.AmountPlayers = list[team.Index - 1];
                }
                RefreshAllTeamTexts();
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        private void OnStopRoundStatsMethod(object[] args)
        {
            StopDeathmatchInfo();
        }

        private void ShowDeathmatchInfo()
        {
            // show info of kills, damage, assists if not already showing
        }

        /*
       mp.events.add( "onClientPlayerAmountInFightSync", ( amountinteam, isroundstarted, amountaliveinteam ) => {
   log( "onClientPlayerAmountInFightSync" );
   roundinfo.amountinteams = [];
   roundinfo.aliveinteams = [];
   amountinteam = JSON.parse( amountinteam );
   if ( isroundstarted == 1)
       amountaliveinteam = JSON.parse( amountaliveinteam );
   for ( let i = 0; i < amountinteam.length; i++ ) {
       roundinfo.amountinteams[i] = Number.parseInt( amountinteam[i] );
       if ( isroundstarted == 0 )
           roundinfo.aliveinteams[i] = Number.parseInt ( amountinteam[i] );
       else
           roundinfo.aliveinteams[i] = Number.parseInt ( amountaliveinteam[i] );
   }
   refreshRoundInfoTeamData();
} );
*/
    }
}
