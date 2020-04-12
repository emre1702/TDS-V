using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Draw.Dx;
using TDS_Client.Handler.Draw.Dx.Grid;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Lobby;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

#pragma warning disable IDE0067 // Dispose objects before losing scope
namespace TDS_Client.Handler.Draw
{
    public class ScoreboardHandler
    {
        public bool IsActivated
        {
            get => _isActivated;
            set
            {
                _isActivated = value;
                _grid.Activated = value;
                if (value)
                    _modAPI.Event.Tick.Add(_tickEventMethod);
                else
                    _modAPI.Event.Tick.Remove(_tickEventMethod);
            }
        }

        private DxGrid _grid;
        private bool _isActivated;
        private ulong _lastLoadedTick;
        private bool _isManualToggleDisabled;

        private readonly DxGridColumn[] _columns = new DxGridColumn[6];

        private readonly EventMethodData<TickDelegate> _tickEventMethod;

        private readonly DxHandler _dxHandler;
        private readonly IModAPI _modAPI;
        private readonly SettingsHandler _settingsHandler;
        private readonly LobbyHandler _lobbyHandler;
        private readonly TimerHandler _timerHandler;
        private readonly RemoteEventsSender _remoteEventsSender;

        public ScoreboardHandler(DxHandler dxHandler, IModAPI modAPI, SettingsHandler settingsHandler, LobbyHandler lobbyHandler, TimerHandler timerHandler, RemoteEventsSender remoteEventsSender,
            EventsHandler eventsHandler)
        {
            _tickEventMethod = new EventMethodData<TickDelegate>(OnTick);

            _dxHandler = dxHandler;
            _modAPI = modAPI;
            _settingsHandler = settingsHandler;
            _lobbyHandler = lobbyHandler;
            _timerHandler = timerHandler;
            _remoteEventsSender = remoteEventsSender;

            eventsHandler.LanguageChanged += (lang, _) => LoadLanguage(lang);
            eventsHandler.LobbyLeft += _ => ReleasedScoreboardKey();
            eventsHandler.LobbyJoinSelectedTeam += () => ReleasedScoreboardKey();

            _grid = new DxGrid(dxHandler, modAPI, 0.5f, 0.5f, 0.45f, 0.365f, Color.FromArgb(187, 10, 10, 10), 0.3f, maxRows: 15);
            CreateColumns();
            CreateTitle();
            CreateBody();
            CreateFooter();
        }

        public void AddMainmenuData(List<SyncedScoreboardMainmenuLobbyDataDto> list)
        {
            if (_grid.Header != null)
                _grid.Header.Activated = false;

            Color backLobbyOfficialColor = Color.FromArgb(187, 190, 190, 190);
            Color backLobbyColor = Color.FromArgb(187, 140, 140, 140);

            list.Sort((a, b) => a.Id < b.Id && a.Id != 0 ? 1 : 0);
            foreach (var entry in list)
            {
                DxGridRow lobbynamerow = new DxGridRow(_dxHandler, _modAPI, _grid, null, entry.IsOfficial ? backLobbyOfficialColor : backLobbyOfficialColor, text: $"{entry.LobbyName} ({entry.PlayersCount})", textAlignment: AlignmentX.Left, scale: 0.3f);
                if (entry.PlayersStr.Length > 0)
                {
                    DxGridRow lobbyplayersrow = new DxGridRow(_dxHandler, _modAPI, _grid, null, Color.DarkGray, text: entry.PlayersStr, scale: 0.25f);
                }
            }
        }

        public void AddLobbyData(List<SyncedScoreboardLobbyDataDto> playerlist, List<SyncedScoreboardMainmenuLobbyDataDto> lobbylist)
        {
            playerlist.Sort((a, b) => a.TeamIndex.CompareTo(b.TeamIndex));
            foreach (var playerdata in playerlist)
            {
                var team = _lobbyHandler.Teams.LobbyTeams[playerdata.TeamIndex];
                DxGridRow row = new DxGridRow(_dxHandler, _modAPI, _grid, null, Color.FromArgb(team.Color.A, team.Color.R, team.Color.G, team.Color.B), textAlignment: AlignmentX.Center, scale: 0.3f);
                new DxGridCell(_dxHandler, _modAPI, playerdata.Name, row, _columns[0]);

                new DxGridCell(_dxHandler, _modAPI, TimeSpan.FromMinutes(playerdata.PlaytimeMinutes).ToString(@"%h\:mm"), row, _columns[1]);
                new DxGridCell(_dxHandler, _modAPI, playerdata.Kills.ToString(), row, _columns[2]);
                new DxGridCell(_dxHandler, _modAPI, playerdata.Assists.ToString(), row, _columns[3]);
                new DxGridCell(_dxHandler, _modAPI, playerdata.Deaths.ToString(), row, _columns[4]);
                new DxGridCell(_dxHandler, _modAPI, team.Name, row, _columns[5]);
            }

            AddMainmenuData(lobbylist);
            if (_grid.Header != null)
                _grid.Header.Activated = true;
        }

        private void LoadLanguage(ILanguage lang)
        {
            if (_grid is null || _grid.Header == null)
                return;

            _grid.Header.Cells[0].SetText(lang.SCOREBOARD_NAME);
            _grid.Header.Cells[1].SetText(lang.SCOREBOARD_PLAYTIME);
            _grid.Header.Cells[2].SetText(lang.SCOREBOARD_KILLS);
            _grid.Header.Cells[3].SetText(lang.SCOREBOARD_ASSISTS);
            _grid.Header.Cells[4].SetText(lang.SCOREBOARD_DEATHS);
            _grid.Header.Cells[5].SetText(lang.SCOREBOARD_TEAM);
        }

        public void PressedScoreboardKey(Control control = Control.Aim)
        {
            if (IsActivated)
                return;
            if (_isManualToggleDisabled && control != Control.Aim)
                return;

            if (control == Control.Aim)
                _isManualToggleDisabled = true;

            _grid.ScrollIndex = 0;
            ulong tick = _timerHandler.ElapsedMs;
            if (tick - _lastLoadedTick >= (ulong)Constants.ScoreboardLoadCooldown || control == Control.Aim)
            {
                _lastLoadedTick = tick;
                if (control == Control.Aim)
                    _remoteEventsSender.SendIgnoreCooldown(ToServerEvent.RequestPlayersForScoreboard);
                else
                    _remoteEventsSender.Send(ToServerEvent.RequestPlayersForScoreboard);
            }

            IsActivated = true;
        }

        public void ReleasedScoreboardKey(Control control = Control.Aim)
        {
            if (!IsActivated)
                return;
            if (_isManualToggleDisabled && control != Control.Aim)
                return;

            if (control == Control.Aim)
                _isManualToggleDisabled = false;
            IsActivated = false;
        }

        public void ClearRows()
        {
            _grid.ClearRows();
        }

        private void OnTick(ulong _)
        {
            _modAPI.Control.DisableControlAction(InputGroup.MOVE, Control.SelectNextWeapon, true);
            _modAPI.Control.DisableControlAction(InputGroup.MOVE, Control.SelectPrevWeapon, true);
        }

        private void CreateColumns()
        {
            _columns[0] = new DxGridColumn(_dxHandler, _modAPI, 0.3f, _grid);
            _columns[1] = new DxGridColumn(_dxHandler, _modAPI, 0.15f, _grid);
            _columns[2] = new DxGridColumn(_dxHandler, _modAPI, 0.1f, _grid);
            _columns[3] = new DxGridColumn(_dxHandler, _modAPI, 0.1f, _grid);
            _columns[4] = new DxGridColumn(_dxHandler, _modAPI, 0.1f, _grid);
            _columns[5] = new DxGridColumn(_dxHandler, _modAPI, 0.25f, _grid);
        }

        private void CreateTitle()
        {
            DxGridRow header = new DxGridRow(_dxHandler, _modAPI, _grid, 0.035f, Color.FromArgb(187, 20, 20, 20), textAlignment: AlignmentX.Center, isHeader: true);
            new DxGridCell(_dxHandler, _modAPI, _settingsHandler.Language.SCOREBOARD_NAME, header, _columns[0]);
            new DxGridCell(_dxHandler, _modAPI, _settingsHandler.Language.SCOREBOARD_PLAYTIME, header, _columns[1]);
            new DxGridCell(_dxHandler, _modAPI, _settingsHandler.Language.SCOREBOARD_KILLS, header, _columns[2]);
            new DxGridCell(_dxHandler, _modAPI, _settingsHandler.Language.SCOREBOARD_ASSISTS, header, _columns[3]);
            new DxGridCell(_dxHandler, _modAPI, _settingsHandler.Language.SCOREBOARD_DEATHS, header, _columns[4]);
            new DxGridCell(_dxHandler, _modAPI, _settingsHandler.Language.SCOREBOARD_TEAM, header, _columns[5]);
        }

        private void CreateBody()
        {
        }

        private void CreateFooter()
        {
        }
    }
}
#pragma warning restore IDE0067 // Dispose objects before losing scope
