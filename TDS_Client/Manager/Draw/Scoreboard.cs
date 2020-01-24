using RAGE.Game;
using RAGE.NUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Instance.Draw.Dx.Grid;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto;
using static RAGE.Events;
using Script = RAGE.Events.Script;

#pragma warning disable IDE0067 // Dispose objects before losing scope
namespace TDS_Client.Manager.Draw
{
    internal class Scoreboard
    {
        private static DxGrid _grid;
        private static bool _isActivated;
        private static ulong _lastLoadedTick;
        private static bool _isManualToggleDisabled;

        private readonly static DxGridColumn[] _columns = new DxGridColumn[6];

        public static bool IsActivated
        {
            get => _isActivated;
            set
            {
                _isActivated = value;
                _grid.Activated = value;
                if (value)
                    Tick += OnTick;
                else
                    Tick -= OnTick;
            }
        }

        static Scoreboard()
        {
            _grid = new DxGrid(0.5f, 0.5f, 0.45f, 0.365f, Color.FromArgb(187, 10, 10, 10), 0.3f, maxRows: 15);
            CreateColumns();
            CreateTitle();
            CreateBody();
            CreateFooter();
        }

        public static void AddMainmenuData(List<SyncedScoreboardMainmenuLobbyDataDto> list)
        {
            if (_grid.Header != null)
                _grid.Header.Activated = false;

            Color backLobbyOfficialColor = Color.FromArgb(187, 190, 190, 190);
            Color backLobbyColor = Color.FromArgb(187, 140, 140, 140);

            list.Sort((a, b) => a.Id < b.Id && a.Id != 0 ? 1 : 0);
            foreach (var entry in list)
            {
                DxGridRow lobbynamerow = new DxGridRow(_grid, null, entry.IsOfficial ? backLobbyOfficialColor : backLobbyOfficialColor, text: $"{entry.LobbyName} ({entry.PlayersCount})", textAlignment: UIResText.Alignment.Left, scale: 0.3f);
                if (entry.PlayersStr.Length > 0)
                {
                    DxGridRow lobbyplayersrow = new DxGridRow(_grid, null, Color.DarkGray, text: entry.PlayersStr, scale: 0.25f);
                }
            }
        }

        public static void AddLobbyData(List<SyncedScoreboardLobbyDataDto> playerlist, List<SyncedScoreboardMainmenuLobbyDataDto> lobbylist)
        {
            playerlist.Sort((a, b) => a.TeamIndex.CompareTo(b.TeamIndex));
            foreach (var playerdata in playerlist)
            {
                var team = Team.CurrentLobbyTeams[playerdata.TeamIndex];
                DxGridRow row = new DxGridRow(_grid, null, Color.FromArgb(team.Color.A, team.Color.R, team.Color.G, team.Color.B), textAlignment: UIResText.Alignment.Centered, scale: 0.3f);
                new DxGridCell(playerdata.Name, row, _columns[0]);

                new DxGridCell(TimeSpan.FromMinutes(playerdata.PlaytimeMinutes).ToString(@"%h\:mm"), row, _columns[1]);
                new DxGridCell(playerdata.Kills.ToString(), row, _columns[2]);
                new DxGridCell(playerdata.Assists.ToString(), row, _columns[3]);
                new DxGridCell(playerdata.Deaths.ToString(), row, _columns[4]);
                new DxGridCell(team.Name, row, _columns[5]);
            }

            AddMainmenuData(lobbylist);
            if (_grid.Header != null)
                _grid.Header.Activated = true;
        }

        public static void LoadLanguage()
        {
            if (_grid is null || _grid.Header == null)
                return;
            _grid.Header.Cells[0].SetText(Settings.Language.SCOREBOARD_NAME);
            _grid.Header.Cells[1].SetText(Settings.Language.SCOREBOARD_PLAYTIME);
            _grid.Header.Cells[2].SetText(Settings.Language.SCOREBOARD_KILLS);
            _grid.Header.Cells[3].SetText(Settings.Language.SCOREBOARD_ASSISTS);
            _grid.Header.Cells[4].SetText(Settings.Language.SCOREBOARD_DEATHS);
            _grid.Header.Cells[5].SetText(Settings.Language.SCOREBOARD_TEAM);
        }

        public static void PressedScoreboardKey(Control control = Control.Aim)
        {
            if (IsActivated)
                return;
            if (_isManualToggleDisabled && control != Control.Aim)
                return;

            if (control == Control.Aim)
                _isManualToggleDisabled = true;

            _grid.ScrollIndex = 0;
            ulong tick = TimerManager.ElapsedTicks;
            if (tick - _lastLoadedTick >= (ulong)ClientConstants.ScoreboardLoadCooldown || control == Control.Aim)
            {
                _lastLoadedTick = tick;
                _grid.ClearRows();
                if (control == Control.Aim)
                    EventsSender.SendIgnoreCooldown(DToServerEvent.RequestPlayersForScoreboard);
                else 
                    EventsSender.Send(DToServerEvent.RequestPlayersForScoreboard);
            }

            IsActivated = true;
        }

        public static void ReleasedScoreboardKey(Control control = Control.Aim)
        {
            if (!IsActivated)
                return;
            if (_isManualToggleDisabled && control != Control.Aim)
                return;

            if (control == Control.Aim)
                _isManualToggleDisabled = false;
            IsActivated = false;
        }

        private static void OnTick(List<TickNametagData> _)
        {
            Pad.DisableControlAction(0, (int)Control.SelectNextWeapon, true);
            Pad.DisableControlAction(0, (int)Control.SelectPrevWeapon, true);
        }

        private static void CreateColumns()
        {
            _columns[0] = new DxGridColumn(0.3f, _grid);
            _columns[1] = new DxGridColumn(0.15f, _grid);
            _columns[2] = new DxGridColumn(0.1f, _grid);
            _columns[3] = new DxGridColumn(0.1f, _grid);
            _columns[4] = new DxGridColumn(0.1f, _grid);
            _columns[5] = new DxGridColumn(0.25f, _grid);
        }

        private static void CreateTitle()
        {
            DxGridRow header = new DxGridRow(_grid, 0.035f, Color.FromArgb(187, 20, 20, 20), textAlignment: UIResText.Alignment.Centered, isHeader: true);
            new DxGridCell(Settings.Language.SCOREBOARD_NAME, header, _columns[0]);
            new DxGridCell(Settings.Language.SCOREBOARD_PLAYTIME, header, _columns[1]);
            new DxGridCell(Settings.Language.SCOREBOARD_KILLS, header, _columns[2]);
            new DxGridCell(Settings.Language.SCOREBOARD_ASSISTS, header, _columns[3]);
            new DxGridCell(Settings.Language.SCOREBOARD_DEATHS, header, _columns[4]);
            new DxGridCell(Settings.Language.SCOREBOARD_TEAM, header, _columns[5]);
        }

        private static void CreateBody()
        {
        }

        private static void CreateFooter()
        {
        }
    }
}
#pragma warning restore IDE0067 // Dispose objects before losing scope
